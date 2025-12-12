<template>
  <!-- ScrollSmoother Wrapper -->
  <div id="smooth-wrapper" class="w-full">
    <!-- ScrollSmoother Content -->
    <div id="smooth-content" class="flex flex-col min-h-screen w-full">
      <ClientOnly>
        <!-- Show IntroLoader as the main content while loading -->
        <IntroLoader v-if="!isIntroLoaderComplete" />

        <!-- Only render page content after intro is complete -->
        <template v-else>
          <UiStagingBadge />
          <Navbar />
          <main class=" min-h-screen">
            <NuxtPage />
          </main>
          <Footer />
        </template>

        <template #fallback>
          <!-- SSR fallback - show loader screen -->
          <div class="min-h-screen w-full bg-white dark:bg-gray-950" />
        </template>
      </ClientOnly>
    </div>
  </div>
</template>

<script setup lang="ts">
import { onMounted } from 'vue'

const { isIntroLoaderComplete, setIntroLoaderComplete } = useIntroLoader()

// Safety fallback: if loader doesn't complete within 10 seconds, show content anyway
onMounted(() => {
  setTimeout(() => {
    if (!isIntroLoaderComplete.value) {
      console.warn('Intro loader timeout - showing content anyway')
      setIntroLoaderComplete(true)
    }
  }, 10000)
})
</script>

<style scoped>
/* Page Transition Overlay - Initial State (GSAP will animate) */
.page-transition-overlay {
  transform: scaleY(0);
  transform-origin: bottom center;
}
</style>
