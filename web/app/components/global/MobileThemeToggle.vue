<template>
  <ClientOnly>
  <button
    @click="cycleTheme"
    class="w-10 h-10 rounded-lg bg-muted flex items-center justify-center hover:bg-accent transition-colors"
    :aria-label="`Current theme: ${colorMode.value}. Click to change.`"
  >
    <!-- Sun Icon (Light Mode) -->
    <svg
      v-if="colorMode.value === 'light'"
      class="w-5 h-5 text-amber-400"
      fill="none"
      stroke="currentColor"
      viewBox="0 0 24 24"
    >
      <path
        stroke-linecap="round"
        stroke-linejoin="round"
        stroke-width="2"
        d="M12 3v1m0 16v1m9-9h-1M4 12H3m15.364 6.364l-.707-.707M6.343 6.343l-.707-.707m12.728 0l-.707.707M6.343 17.657l-.707.707M16 12a4 4 0 11-8 0 4 4 0 018 0z"
      />
    </svg>

    <!-- Moon Icon (Dark Mode) -->
    <svg
      v-else-if="colorMode.value === 'dark'"
      class="w-5 h-5 text-blue-400"
      fill="none"
      stroke="currentColor"
      viewBox="0 0 24 24"
    >
      <path
        stroke-linecap="round"
        stroke-linejoin="round"
        stroke-width="2"
        d="M20.354 15.354A9 9 0 018.646 3.646 9.003 9.003 0 0012 21a9.003 9.003 0 008.354-5.646z"
      />
    </svg>

    <!-- System Icon -->
    <svg
      v-else
      class="w-5 h-5"
      fill="none"
      stroke="currentColor"
      viewBox="0 0 24 24"
    >
      <path
        stroke-linecap="round"
        stroke-linejoin="round"
        stroke-width="2"
        d="M9.75 17L9 20l-1 1h8l-1-1-.75-3M3 13h18M5 17h14a2 2 0 002-2V5a2 2 0 00-2-2H5a2 2 0 00-2 2v10a2 2 0 002 2z"
      />
    </svg>
  </button>
    <template #fallback>
      <!-- SSR fallback - matches the default theme (system) -->
      <button
        class="w-10 h-10 rounded-lg bg-muted flex items-center justify-center hover:bg-accent transition-colors"
        aria-label="Loading theme toggle..."
      >
        <svg
          class="w-5 h-5"
          fill="none"
          stroke="currentColor"
          viewBox="0 0 24 24"
        >
          <path
            stroke-linecap="round"
            stroke-linejoin="round"
            stroke-width="2"
            d="M9.75 17L9 20l-1 1h8l-1-1-.75-3M3 13h18M5 17h14a2 2 0 002-2V5a2 2 0 00-2-2H5a2 2 0 00-2 2v10a2 2 0 002 2z"
          />
        </svg>
      </button>
    </template>
  </ClientOnly>
</template>

<script setup lang="ts">
const colorMode = useColorMode()

const themes = ['dark', 'light', 'system'] as const

const cycleTheme = () => {
  const currentIndex = themes.indexOf(colorMode.preference as typeof themes[number])
  const nextIndex = (currentIndex + 1) % themes.length
  colorMode.preference = themes[nextIndex]
}
</script>
