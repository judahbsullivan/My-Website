<template>
  <div class="min-h-screen pt-24 pb-12">
    <SectionsDashboard />
  </div>
</template>

<script setup lang="ts">
// Protect this route with admin middleware
definePageMeta({
  middleware: 'admin'
})

useHead({
  title: 'Dashboard | Admin | Judah Sullivan',
})
</script>

