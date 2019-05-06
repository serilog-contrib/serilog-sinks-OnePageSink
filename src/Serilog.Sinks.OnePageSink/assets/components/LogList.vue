
<template id="log-list">
    <section class="events">
        <h1>Events</h1>
        <div class="log-list__checkbox">
            <input id="checkbox" type="checkbox" v-on:change="onReceiveChange" v-model="receiveMessages">
            <label for="checkbox">Receive messages</label>
        </div>
        <transition-group name="fade" tag="ul" class="log-list">
            <log-entry v-for="(entry, index) in events"
                       :entry="entry"
                       :key="entry.id">
            </log-entry>
        </transition-group>
    </section>
</template>

<script>
    const { Subject } = rxjs;
    const { bufferTime } = rxjs.operators;
    import * as signalR from '@aspnet/signalr';
    import LogEntry from './LogEntry.vue'



    export default {
        template: '#log-list',
        props: {
            events: { default: [] }
        },
        components: {
            LogEntry
        },
        data() {
            return {
                receiveMessages: true,
                maximumItems: 50
            };
        },
        created: function () {
            var thisLogList = this;
            this.connection = new signalR.HubConnectionBuilder().withUrl("/logevents").build();
            this.subject = new Subject();
            this.subject
                .pipe(bufferTime(500))
                .subscribe(messages => {
                    if (messages.length === 0) {
                        return;
                    }
                    messages.map(x => {
                        var time = new Date(x.logEvents.timestamp)
                        return {
                            id: time.getTime(),
                            timestamp: time,
                            message: x.message,
                            details: x.logEvents
                        };
                    }).forEach(x => {
                        thisLogList.events.unshift(x);
                    });
                    thisLogList.events.length = Math.min(thisLogList.events.length, thisLogList.maximumItems);
                });

        },
        mounted: function () {
            var thisLogList = this;
            this.connection.start();
            this.connection.on("LogMessage",
                function (message, logEvents) {
                    const item = {
                        message: message,
                        logEvents: logEvents
                    };
                    thisLogList.subject.next(item);
                });

        },
        methods: {
            onReceiveChange() {
                if (this.receiveMessages == false) {
                    this.connection.stop();
                    return;
                }
                this.connection.start();
            },
            clearAll() {
                this.events = [];
            },
        }
    };
</script>

<style>

    .fade-enter-active {
        transition: background-color 0.3s ease-out;
    }

    .fade-enter {
        background: #F9E79F;
        opacity: 0;
    }
</style>