<template>
  <Teleport to="body">
    <div v-if="shouldShow" class="staging-badge-container">
      <div class="staging-badge">
        <span class="w-2 h-2 rounded-full bg-amber-500 animate-pulse shrink-0" />
        <div class="badge-text-container">
          <Transition name="fade" mode="out-in">
            <span 
              :key="currentIndex"
              class="text-xs font-medium text-foreground whitespace-nowrap"
            >
              {{ messages[currentIndex] }}
            </span>
          </Transition>
        </div>
      </div>
    </div>
  </Teleport>
</template>

<script setup lang="ts">
import { ref, computed, onMounted, onUnmounted } from 'vue'

const config = useRuntimeConfig()

// Only show when deployed from 'staging' branch, not 'main' branch
const shouldShow = computed(() => {
  const deployBranch = config.public.deployBranch?.toLowerCase() || 'main'
  // Show badge only if deployed from staging branch
  return deployBranch === 'staging'
})

const messages = ['Staging Site', 'Under Construction']
const currentIndex = ref(0)

let intervalId: NodeJS.Timeout | null = null

onMounted(() => {
  // Only start interval if badge should be shown
  if (!shouldShow.value) return
  
  // Rotate messages every 3 seconds (slow enough to be read)
  intervalId = setInterval(() => {
    currentIndex.value = (currentIndex.value + 1) % messages.length
  }, 3000)
})

onUnmounted(() => {
  if (intervalId) {
    clearInterval(intervalId)
  }
})
</script>

<style scoped>
.staging-badge-container {
  position: fixed !important;
  bottom: .5rem !important;
  right: .5rem !important;
  z-index: 9999 !important;
  pointer-events: none;
}

.staging-badge {
  display: inline-flex;
  align-items: center;
  gap: 0.5rem;
  border-radius: 9999px;
  border: 1px solid var(--border);
  background: var(--card);
  opacity: 0.95;
  backdrop-filter: blur(8px);
  padding: 0.5rem 1rem;
  box-shadow: 0 10px 15px -3px rgba(0, 0, 0, 0.1), 0 4px 6px -2px rgba(0, 0, 0, 0.05);
  pointer-events: auto;
}

.badge-text-container {
  min-width: 120px;
  text-align: left;
  height: 1.25rem;
  display: flex;
  align-items: center;
}

.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.8s ease-in-out;
}

.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}
</style>
