// Backward compatibility - re-export from unified usePageAnimations
// This allows existing code to continue working without changes

import {
  registerEntranceAnimation as _registerEntranceAnimation,
  unregisterPageAnimation as _unregisterPageAnimation,
  runAllEntranceAnimations as _runAllEntranceAnimations
} from './usePageAnimations'

// Re-export with original names
export const registerEntranceAnimation = _registerEntranceAnimation
export const unregisterEntranceAnimation = _unregisterPageAnimation
export const runAllEntranceAnimations = _runAllEntranceAnimations

// Re-export the composable wrapper
export function usePageEntranceAnimations() {
  return {
    registerEntranceAnimation: _registerEntranceAnimation,
    unregisterEntranceAnimation: _unregisterPageAnimation,
    runAllEntranceAnimations: _runAllEntranceAnimations
  }
}
