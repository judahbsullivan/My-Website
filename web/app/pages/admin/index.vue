<template>
  <div></div>
</template>

<script setup lang="ts">
// Redirect to dashboard
definePageMeta({
  middleware: 'admin'
})

// This will redirect via middleware, but we can also do it here as a fallback
onMounted(() => {
  navigateTo('/admin/dashboard')
})
</script>

