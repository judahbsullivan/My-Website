<template>
  <div
    class="relative inline-block"
    @mouseenter="handleMouseEnter"
    @mouseleave="handleMouseLeave"
  >
    <slot name="trigger" />
    <Transition
      enter-active-class="transition ease-out duration-200"
      enter-from-class="opacity-0 translate-y-1"
      enter-to-class="opacity-100 translate-y-0"
      leave-active-class="transition ease-in duration-150"
      leave-from-class="opacity-100 translate-y-0"
      leave-to-class="opacity-0 translate-y-1"
    >
      <div
        v-if="show"
        :class="[
          'absolute z-50 px-3 py-1.5 text-xs font-medium text-white bg-gray-900 dark:bg-gray-800 rounded-lg shadow-lg',
          'whitespace-nowrap pointer-events-none',
          placementClasses
        ]"
      >
        {{ text }}
        <div
          :class="[
            'absolute w-2 h-2 bg-gray-900 dark:bg-gray-800 transform rotate-45',
            arrowClasses
          ]"
        />
      </div>
    </Transition>
  </div>
</template>

<script setup lang="ts">
interface Props {
  text: string
  placement?: 'top' | 'bottom' | 'left' | 'right'
  show?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  placement: 'top',
  show: undefined
})

const isHovered = ref(false)

const show = computed(() => {
  return props.show !== undefined ? props.show : isHovered.value
})

const placementClasses = computed(() => {
  const classes: Record<string, string> = {
    top: 'bottom-full left-1/2 -translate-x-1/2 mb-2',
    bottom: 'top-full left-1/2 -translate-x-1/2 mt-2',
    left: 'right-full top-1/2 -translate-y-1/2 mr-2',
    right: 'left-full top-1/2 -translate-y-1/2 ml-2'
  }
  return classes[props.placement] || classes.top
})

const arrowClasses = computed(() => {
  const classes: Record<string, string> = {
    top: 'bottom-0 left-1/2 -translate-x-1/2 translate-y-1/2',
    bottom: 'top-0 left-1/2 -translate-x-1/2 -translate-y-1/2',
    left: 'right-0 top-1/2 -translate-y-1/2 translate-x-1/2',
    right: 'left-0 top-1/2 -translate-y-1/2 -translate-x-1/2'
  }
  return classes[props.placement] || classes.top
})

const handleMouseEnter = () => {
  isHovered.value = true
}

const handleMouseLeave = () => {
  isHovered.value = false
}
</script>

