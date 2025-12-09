// Composable to manage page exit animations
// Components register their exit animations, and the page transition calls them

type ExitAnimation = () => Promise<void>

// Singleton map - shared across all uses of this composable
const exitAnimations: Map<string, ExitAnimation> = new Map()

// Register an exit animation for a component
export function registerExitAnimation(key: string, animation: ExitAnimation) {
  console.log('[PageExit] Registering:', key)
  exitAnimations.set(key, animation)
}

// Unregister an exit animation
export function unregisterExitAnimation(key: string) {
  console.log('[PageExit] Unregistering:', key)
  exitAnimations.delete(key)
}

// Run all registered exit animations in parallel
export async function runAllExitAnimations(): Promise<void> {
  const keys = Array.from(exitAnimations.keys())
  const animations = Array.from(exitAnimations.values())
  
  console.log('[PageExit] Running exit animations for:', keys)
  
  if (animations.length === 0) {
    console.log('[PageExit] No animations registered!')
    return
  }

  try {
    await Promise.all(animations.map(fn => fn()))
    console.log('[PageExit] All animations completed')
  } catch (e) {
    console.error('[PageExit] Animation error:', e)
  }
}

// Clear all registered animations (for cleanup)
export function clearAllExitAnimations() {
  exitAnimations.clear()
}

// Composable wrapper for convenience
export function usePageExitAnimations() {
  return {
    registerExitAnimation,
    unregisterExitAnimation,
    runAllExitAnimations,
    clearAllExitAnimations
  }
}
