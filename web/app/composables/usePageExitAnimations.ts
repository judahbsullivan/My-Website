// Backward compatibility - re-export from unified usePageAnimations
// This allows existing code to continue working without changes

import {
  registerExitAnimation as _registerExitAnimation,
  unregisterPageAnimation as _unregisterPageAnimation,
  runAllExitAnimations as _runAllExitAnimations,
  clearAllAnimations as _clearAllAnimations
} from './usePageAnimations'

// Re-export with original names
export const registerExitAnimation = _registerExitAnimation
export const unregisterExitAnimation = _unregisterPageAnimation
export const runAllExitAnimations = _runAllExitAnimations
export const clearAllExitAnimations = _clearAllAnimations

// Re-export the composable wrapper
export function usePageExitAnimations() {
  return {
    registerExitAnimation: _registerExitAnimation,
    unregisterExitAnimation: _unregisterPageAnimation,
    runAllExitAnimations: _runAllExitAnimations,
    clearAllExitAnimations: _clearAllAnimations
  }
}
