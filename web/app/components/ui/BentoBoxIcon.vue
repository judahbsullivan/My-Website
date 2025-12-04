<template>
  <div :class="iconContainerClasses">
    <Icon :name="icon" :class="iconClasses" />
  </div>
</template>

<script setup lang="ts">
interface Props {
  icon: string
  variant?: 'primary' | 'secondary' | 'success' | 'warning' | 'danger' | 'info' | 'default'
  size?: 'sm' | 'md' | 'lg'
}

const props = withDefaults(defineProps<Props>(), {
  variant: 'primary',
  size: 'md'
})

const sizeClasses = {
  sm: {
    container: 'w-6 h-6',
    icon: 'w-3 h-3'
  },
  md: {
    container: 'w-8 h-8',
    icon: 'w-4 h-4'
  },
  lg: {
    container: 'w-14 h-14',
    icon: 'w-5 h-5'
  }
}

const variantColors = {
  primary: 'text-primary bg-primary/20 group-hover:bg-primary/30',
  secondary: 'text-blue-500 bg-blue-500/20 group-hover:bg-blue-500/30',
  success: 'text-success bg-success/20 group-hover:bg-success/30',
  warning: 'text-amber-500 bg-amber-500/20 group-hover:bg-amber-500/30',
  danger: 'text-destructive bg-destructive/20 group-hover:bg-destructive/30',
  info: 'text-purple-500 bg-purple-500/20 group-hover:bg-purple-500/30',
  default: 'text-foreground bg-muted/20 group-hover:bg-muted/30'
}

const iconContainerClasses = computed(() => {
  return [
    sizeClasses[props.size].container,
    'rounded-lg flex items-center justify-center transition-colors shrink-0',
    'self-center', // Ensure icon container is centered vertically
    variantColors[props.variant]
  ].join(' ')
})

const iconClasses = computed(() => {
  return sizeClasses[props.size].icon
})
</script>

