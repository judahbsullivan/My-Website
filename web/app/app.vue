<template>
  <NuxtLayout>
    <NuxtPage />
  </NuxtLayout>
</template>

<style>
/* Page Transition Styles */
.page-enter-active,
.page-leave-active {
  transition: all 0.4s cubic-bezier(0.645, 0.045, 0.355, 1);
}

.page-enter-from,
.page-leave-to {
  opacity: 0;
  filter: blur(1rem);
}
</style>
