// Unified composable to manage page animations (exit and entrance)
// Components register both their exit and entrance animations together

type ExitAnimation = () => Promise<void>
type EntranceAnimation = () => void

interface ComponentAnimations {
  exit: ExitAnimation
  entrance: EntranceAnimation
}

// Singleton maps - shared across all uses of this composable
const componentAnimations: Map<string, ComponentAnimations> = new Map()

// Register both exit and entrance animations for a component
export function registerPageAnimation(key: string, exit: ExitAnimation, entrance: EntranceAnimation) {
  componentAnimations.set(key, { exit, entrance })
}

// Unregister animations for a component
export function unregisterPageAnimation(key: string) {
  componentAnimations.delete(key)
}

// Run all registered exit animations in parallel
export async function runAllExitAnimations(): Promise<void> {
  const keys = Array.from(componentAnimations.keys())
  const animations = Array.from(componentAnimations.values())
  
  if (animations.length === 0) {
    return
  }

  try {
    await Promise.all(animations.map(anim => anim.exit()))
  } catch (e) {
    console.error('[PageAnimations] Exit animation error:', e)
  }
}

// Run all registered entrance animations
export function runAllEntranceAnimations(): void {
  const animations = Array.from(componentAnimations.values())
  
  if (animations.length === 0) {
    return
  }

  animations.forEach(anim => {
    try {
      anim.entrance()
    } catch (e) {
      console.error('[PageAnimations] Entrance animation error:', e)
    }
  })
}

// Clear all registered animations (for cleanup)
export function clearAllAnimations() {
  componentAnimations.clear()
}

// Composable wrapper for convenience
export function usePageAnimations() {
  return {
    registerPageAnimation,
    unregisterPageAnimation,
    runAllExitAnimations,
    runAllEntranceAnimations,
    clearAllAnimations
  }
}

// Backward compatibility - keep old exit animations API working
export function registerExitAnimation(key: string, animation: ExitAnimation) {
  // If component already registered, update exit only
  const existing = componentAnimations.get(key)
  if (existing) {
    existing.exit = animation
  } else {
    // Create placeholder entrance that does nothing
    componentAnimations.set(key, {
      exit: animation,
      entrance: () => {}
    })
  }
}

export function unregisterExitAnimation(key: string) {
  unregisterPageAnimation(key)
}

// Backward compatibility - keep old entrance animations API working
export function registerEntranceAnimation(key: string, animation: EntranceAnimation) {
  // If component already registered, update entrance only
  const existing = componentAnimations.get(key)
  if (existing) {
    existing.entrance = animation
  } else {
    // Create placeholder exit that resolves immediately
    componentAnimations.set(key, {
      exit: async () => {},
      entrance: animation
    })
  }
}

export function unregisterEntranceAnimation(key: string) {
  unregisterPageAnimation(key)
}
