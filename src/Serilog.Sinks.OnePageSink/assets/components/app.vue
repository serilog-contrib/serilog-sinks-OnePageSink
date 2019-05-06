
<template>
    <div id="main-app">
        <log-list :events="events"></log-list>
    </div>
</template>

<script>
    import LogList from "./LogList.vue"

    export default {
        name: 'App',
        components: {
            LogList
        },
        data() {
            return {
                events: []
            }
        }
    }
</script>