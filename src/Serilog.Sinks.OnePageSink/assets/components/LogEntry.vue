
<template>
    <li class="entry">

        <div class="entry-item entry-item__timespan">{{ entry.timestamp.toLocaleString() }}</div>
        <div class="entry-item entry-item__separator"></div>
        <div class="entry-item entry-item__message" v-on:click="toggle">{{ entry.message }}</div>
        <div class="entry-details" v-bind:class="{ hidden: isHidden }">
            <pre>
{{entry.details}}
</pre>
        </div>
    </li>
</template>

<script>

    export default {
        name: 'LogEntry',
        props: ['entry'],
        data() {
            return {
                isHidden: true
            };
        },
        methods: {
            toggle() {
                this.isHidden = !this.isHidden;
            }
        }
    }
</script>

<style>
    .entry {
        border-bottom: 1px solid #F8F8F8;
    }

    .entry-item {
        display: inline-block;
    }

    .entry-item__separator {
        background-color: #F8F8F8;
        width: 1px;
    }

    .hidden {
        display: none;
    }
</style>
