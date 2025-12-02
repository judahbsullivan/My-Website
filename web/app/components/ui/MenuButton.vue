<template>
    <button 
    @click="props.onClick"
    class=" flex md:hidden flex-col items-center gap-2">
    Menu
    </button>
</template>

<script setup lang="ts">
interface Props {
  onClick: () => void
}

const props = defineProps<Props>()
</script>