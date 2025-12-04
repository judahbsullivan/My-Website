<template>
  <component
    :is="to ? 'NuxtLink' : 'div'"
    :to="to"
    :class="bentoClasses"
    :style="bentoStyles"
  >
    <slot />
  </component>
</template>

<script setup lang="ts">
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

// Variant background classes - subtle theme colors that don't overpower content
const variantBackgroundClasses = {
  primary: 'bg-primary/10',
  secondary: 'bg-blue-500/10',
  success: 'bg-success/10',
  warning: 'bg-amber-500/10',
  danger: 'bg-destructive/10',
  info: 'bg-purple-500/10',
  default: 'bg-muted/30'
}

// Variant background classes for containers - lighter so content is visible
const variantContainerBackgroundClasses = {
  primary: 'bg-primary/8',
  secondary: 'bg-blue-500/8',
  success: 'bg-success/8',
  warning: 'bg-amber-500/8',
  danger: 'bg-destructive/8',
  info: 'bg-purple-500/8',
  default: 'bg-muted/20'
}

// Variant gradient classes (for gradient mode)
const variantGradientClasses = {
  primary: 'from-primary/15 via-primary/8 to-primary/5',
  secondary: 'from-blue-500/15 via-blue-500/8 to-blue-500/5',
  success: 'from-success/15 via-success/8 to-success/5',
  warning: 'from-amber-500/15 via-amber-500/8 to-amber-500/5',
  danger: 'from-destructive/15 via-destructive/8 to-destructive/5',
  info: 'from-purple-500/15 via-purple-500/8 to-purple-500/5',
  default: 'from-muted/25 via-muted/15 to-muted/8'
}

// Variant gradient classes for containers - lighter so content is visible
const variantContainerGradientClasses = {
  primary: 'from-primary/12 via-primary/8 to-primary/5',
  secondary: 'from-blue-500/12 via-blue-500/8 to-blue-500/5',
  success: 'from-success/12 via-success/8 to-success/5',
  warning: 'from-amber-500/12 via-amber-500/8 to-amber-500/5',
  danger: 'from-destructive/12 via-destructive/8 to-destructive/5',
  info: 'from-purple-500/12 via-purple-500/8 to-purple-500/5',
  default: 'from-muted/20 via-muted/12 to-muted/8'
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
      props.border ? 'border border-border/50' : '',
      props.shadow ? 'shadow-lg' : '',
      props.hover ? 'hover:border-primary/50 transition-all duration-300' : ''
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

  // Add border if enabled
  if (props.border) {
    classes.push('border border-border/30')
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

