<template>
  <NuxtLayout>
    <NuxtPage />
  </NuxtLayout>
</template>

<style>
/* Disable global page fade/blur transitions to avoid flicker */
.page-enter-active,
.page-leave-active {
  transition: none;
}

.page-enter-from,
.page-leave-to {
  opacity: 1;
  filter: none;
}
</style>
