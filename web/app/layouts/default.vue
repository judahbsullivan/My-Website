<template>
  <div class="flex flex-col min-h-screen w-full overflow-hidden">
    <Navbar />
    <main>
      <slot />
    </main>
    <Footer />

    <!-- Page Transition Overlay - CSS Mask Effect -->
    <div class="page-transition-overlay fixed inset-0 z-99999 bg-black pointer-events-none" aria-hidden="true" />

    <!-- Intro Loader -->
    <IntroLoader />
  </div>
</template>

<script setup lang="ts">
// Ensure layout always renders consistently
// No client-only logic here to prevent hydration mismatches
</script>

<style scoped>
/* Page Transition Overlay - Initial State (GSAP will animate) */
.page-transition-overlay {
  transform: scaleY(0);
  transform-origin: bottom center;
}
</style>
