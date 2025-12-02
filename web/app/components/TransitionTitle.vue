<template>
  <div
    ref="titleElementRef"
    class="text-white font-bold text-6xl md:text-8xl lg:text-9xl relative z-9999"
    style="font-family: system-ui, sans-serif; letter-spacing: 0.05em; visibility: visible; display: block; opacity: 1;"
  >
    {{ title }}
  </div>
</template>

<script setup lang="ts">
interface Props {
  title: string
}

const props = defineProps<Props>()
const titleElementRef = ref<HTMLElement | null>(null)

defineExpose({
  getElement: () => titleElementRef.value,
  element: titleElementRef,
})
</script>


