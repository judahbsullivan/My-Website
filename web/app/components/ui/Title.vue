<template>
  <component
    :is="as"
    :class="titleClasses"
  >
    <slot />
  </component>
</template>

<script setup lang="ts">
import { computed } from 'vue'

interface Props {
  as?: 'h1' | 'h2' | 'h3' | 'h4' | 'h5' | 'h6' | 'p' | 'span' | 'div'
  size?: 'xs' | 'sm' | 'md' | 'lg' | 'xl' | '2xl' | '3xl' | '4xl' | '5xl' | '6xl'
  weight?: 'normal' | 'medium' | 'semibold' | 'bold'
  align?: 'left' | 'center' | 'right'
  className?: string
}

const props = withDefaults(defineProps<Props>(), {
  as: 'h1',
  size: '4xl',
  weight: 'bold',
  align: 'left',
  className: ''
})

const sizeClasses = computed(() => {
  const sizes = {
    xs: 'text-2xl',
    sm: 'text-3xl',
    md: 'text-4xl lg:text-5xl',
    lg: 'text-4xl sm:text-5xl lg:text-6xl',
    xl: 'text-5xl sm:text-6xl lg:text-7xl',
    '2xl': 'text-6xl sm:text-7xl lg:text-8xl',
    '3xl': 'text-4xl sm:text-6xl lg:text-8xl',
    '4xl': 'text-5xl sm:text-7xl lg:text-9xl',
    '5xl': 'text-6xl sm:text-8xl lg:text-[10rem]',
    '6xl': 'text-7xl sm:text-9xl lg:text-[12rem]'
  }
  return sizes[props.size]
})

const weightClasses = computed(() => {
  const weights = {
    normal: 'font-normal',
    medium: 'font-medium',
    semibold: 'font-semibold',
    bold: 'font-bold'
  }
  return weights[props.weight]
})

const alignClasses = computed(() => {
  const aligns = {
    left: 'text-left',
    center: 'text-center',
    right: 'text-right'
  }
  return aligns[props.align]
})

const titleClasses = computed(() => [
  'magazine-title',
  sizeClasses.value,
  weightClasses.value,
  alignClasses.value,
  'tracking-tight leading-[0.9]',
  'antialiased',
  props.className
])
</script>

<style scoped>
.magazine-title {
  letter-spacing: -0.02em;
  font-feature-settings: 'kern' 1, 'liga' 1;
}
</style>

