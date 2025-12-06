<template>
  <component
    :is="to ? 'NuxtLink' : 'div'"
    ref="elementRef"
    :to="to"
    :class="bentoClasses"
    :style="bentoStyles"
  >
    <slot />
  </component>
</template>

<script setup lang="ts">
import { ref } from 'vue'

// Expose element ref for external animation access
const elementRef = ref<HTMLElement | null>(null)
defineExpose({ el: elementRef })

interface Props {
  to?: string
  href?: string
  height?: string | number
  width?: string | number
  size?: 'sm' | 'md' | 'lg' | 'xl' | 'full'
  variant?: 'primary' | 'secondary' | 'success' | 'warning' | 'danger' | 'info' | 'default'
  gradient?: boolean
  border?: boolean
  hover?: boolean
  padding?: 'none' | 'sm' | 'md' | 'lg' | 'xl'
  rounded?: 'sm' | 'md' | 'lg' | 'xl' | '2xl'
  className?: string
  colSpan?: number
  // New props for container mode
  container?: boolean
  backdrop?: boolean
  shadow?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  height: undefined,
  width: undefined,
  size: 'md',
  variant: 'default',
  gradient: true,
  border: true,
  hover: true,
  padding: 'md',
  rounded: 'xl',
  className: '',
  colSpan: undefined,
  container: false,
  backdrop: false,
  shadow: false
})

// Size presets - using min-height to allow content to expand
const sizeClasses = {
  sm: 'min-h-[56px]',
  md: 'min-h-[88px]',
  lg: 'min-h-[120px]',
  xl: 'min-h-[160px]',
  full: 'h-full'
}

// Variant background classes - better contrast for dark/light modes
const variantBackgroundClasses = {
  primary: 'bg-primary/15 dark:bg-primary/20',
  secondary: 'bg-blue-500/15 dark:bg-blue-500/20',
  success: 'bg-success/15 dark:bg-success/20',
  warning: 'bg-amber-500/15 dark:bg-amber-500/20',
  danger: 'bg-destructive/15 dark:bg-destructive/20',
  info: 'bg-purple-500/15 dark:bg-purple-500/20',
  default: 'bg-card dark:bg-card/80'
}

// Variant background classes for containers - more visible backgrounds
const variantContainerBackgroundClasses = {
  primary: 'bg-primary/12 dark:bg-primary/18',
  secondary: 'bg-blue-500/12 dark:bg-blue-500/18',
  success: 'bg-success/12 dark:bg-success/18',
  warning: 'bg-amber-500/12 dark:bg-amber-500/18',
  danger: 'bg-destructive/12 dark:bg-destructive/18',
  info: 'bg-purple-500/12 dark:bg-purple-500/18',
  default: 'bg-card dark:bg-card/90'
}

// Variant gradient classes (for gradient mode) - better contrast
const variantGradientClasses = {
  primary: 'from-primary/20 via-primary/12 to-primary/8 dark:from-primary/25 dark:via-primary/18 dark:to-primary/12',
  secondary: 'from-blue-500/20 via-blue-500/12 to-blue-500/8 dark:from-blue-500/25 dark:via-blue-500/18 dark:to-blue-500/12',
  success: 'from-success/20 via-success/12 to-success/8 dark:from-success/25 dark:via-success/18 dark:to-success/12',
  warning: 'from-amber-500/20 via-amber-500/12 to-amber-500/8 dark:from-amber-500/25 dark:via-amber-500/18 dark:to-amber-500/12',
  danger: 'from-destructive/20 via-destructive/12 to-destructive/8 dark:from-destructive/25 dark:via-destructive/18 dark:to-destructive/12',
  info: 'from-purple-500/20 via-purple-500/12 to-purple-500/8 dark:from-purple-500/25 dark:via-purple-500/18 dark:to-purple-500/12',
  default: 'from-card via-card/90 to-card/80 dark:from-card dark:via-card/95 dark:to-card/90'
}

// Variant gradient classes for containers - more visible
const variantContainerGradientClasses = {
  primary: 'from-primary/18 via-primary/12 to-primary/8 dark:from-primary/22 dark:via-primary/18 dark:to-primary/14',
  secondary: 'from-blue-500/18 via-blue-500/12 to-blue-500/8 dark:from-blue-500/22 dark:via-blue-500/18 dark:to-blue-500/14',
  success: 'from-success/18 via-success/12 to-success/8 dark:from-success/22 dark:via-success/18 dark:to-success/14',
  warning: 'from-amber-500/18 via-amber-500/12 to-amber-500/8 dark:from-amber-500/22 dark:via-amber-500/18 dark:to-amber-500/14',
  danger: 'from-destructive/18 via-destructive/12 to-destructive/8 dark:from-destructive/22 dark:via-destructive/18 dark:to-destructive/14',
  info: 'from-purple-500/18 via-purple-500/12 to-purple-500/8 dark:from-purple-500/22 dark:via-purple-500/18 dark:to-purple-500/14',
  default: 'from-card via-card/95 to-card/90 dark:from-card dark:via-card/98 dark:to-card/95'
}

// Padding classes - increased for better content spacing
const paddingClasses = {
  none: 'p-0',
  sm: 'p-3',
  md: 'p-4',
  lg: 'p-5',
  xl: 'p-6'
}

// Rounded classes
const roundedClasses = {
  sm: 'rounded-sm',
  md: 'rounded-md',
  lg: 'rounded-lg',
  xl: 'rounded-xl',
  '2xl': 'rounded-2xl'
}

// Computed classes
const bentoClasses = computed(() => {
  const classes: string[] = []
  
  // Container mode - for wrapping content sections
  if (props.container) {
    classes.push(
      roundedClasses[props.rounded],
      paddingClasses[props.padding],
      props.backdrop ? 'backdrop-blur-xl' : '',
      props.border ? 'border border-border/50 dark:border-border/70' : '',
      props.shadow ? 'shadow-lg dark:shadow-xl' : '',
      props.hover ? 'hover:border-primary/50 dark:hover:border-primary/70 transition-all duration-300' : ''
    )
    
    // Add background color based on variant - more visible for containers
    // Only add if className doesn't already have a background color
    const hasBgClass = props.className?.match(/\bbg-[\w-]+\b/)
    if (!hasBgClass) {
      if (props.gradient) {
        classes.push(`bg-gradient-to-br ${variantContainerGradientClasses[props.variant]}`)
      } else {
        classes.push(variantContainerBackgroundClasses[props.variant])
      }
    }
    
    // Add className last so it can override variant backgrounds
    if (props.className) {
      classes.push(props.className)
    }
    
    return classes.filter(Boolean).join(' ')
  }
  
  // Regular BentoBox mode
  // Determine flex direction from className or default to column
  const flexDirection = props.className?.includes('flex-row') ? 'flex-row' : 'flex-col'
  const gap = flexDirection === 'flex-row' ? 'gap-2.5' : 'gap-1.5'
  
  classes.push(
    roundedClasses[props.rounded],
    paddingClasses[props.padding],
    `flex ${flexDirection} items-center justify-center ${gap}`,
    'transition-colors cursor-pointer',
    'overflow-hidden' // Prevent content overflow
  )
  
  // Ensure proper vertical alignment for flex-row
  if (flexDirection === 'flex-row') {
    classes.push('items-center') // Force items-center for row layout
  }

  // Add background color based on variant - subtle theme colors
  // Only add if className doesn't already have a background color
  const hasBgClass = props.className?.match(/\bbg-[\w-]+\b/)
  if (!hasBgClass) {
    if (props.gradient) {
      classes.push(`bg-gradient-to-br ${variantGradientClasses[props.variant]}`)
    } else {
      classes.push(variantBackgroundClasses[props.variant])
    }
  }
  
  // Add className last so it can override variant backgrounds
  if (props.className) {
    classes.push(props.className)
  }

  // Add border if enabled - better visibility in dark mode
  if (props.border) {
    classes.push('border border-border/40 dark:border-border/60')
  }

  // Add hover effect if enabled
  if (props.hover) {
    const hoverColors = {
      primary: 'hover:border-primary/50',
      secondary: 'hover:border-blue-500/50',
      success: 'hover:border-success/50',
      warning: 'hover:border-amber-500/50',
      danger: 'hover:border-destructive/50',
      info: 'hover:border-purple-500/50',
      default: 'hover:border-primary/50'
    }
    classes.push(hoverColors[props.variant])
    classes.push('group')
  }

  // Add size class if height not specified
  if (!props.height) {
    classes.push(sizeClasses[props.size])
  }

  // Add col-span if specified
  if (props.colSpan) {
    classes.push(`col-span-${props.colSpan}`)
  }

  return classes.join(' ')
})

// Computed styles
const bentoStyles = computed(() => {
  const styles: Record<string, string> = {}
  
  if (props.height) {
    // Use min-height to allow content to expand if needed
    if (typeof props.height === 'number') {
      styles.minHeight = `${props.height}px`
    } else {
      // If it's a string like "100px" or "5rem", use it directly
      if (props.height.includes('px') || props.height.includes('rem') || props.height.includes('em')) {
        styles.minHeight = props.height
      } else {
        // Otherwise assume it's a Tailwind class and don't set inline style
        styles.minHeight = props.height
      }
    }
  }
  
  if (props.width) {
    if (typeof props.width === 'number') {
      styles.width = `${props.width}px`
    } else {
      styles.width = props.width
    }
  }
  
  return styles
})
</script>

