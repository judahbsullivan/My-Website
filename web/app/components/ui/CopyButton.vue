<template>
  <button
    @click="handleCopy"
    :class="[
      'inline-flex items-center gap-2 px-3 py-1.5 text-xs font-medium rounded-lg transition-colors',
      'bg-card border border-border hover:border-primary/50 hover:bg-primary/10',
      'text-foreground hover:text-primary',
      copied ? 'border-success text-success' : ''
    ]"
    :disabled="copied"
  >
    <Icon :name="copied ? 'heroicons:check' : 'heroicons:clipboard'" class="w-4 h-4" />
    <span>{{ copied ? 'Copied!' : 'Copy' }}</span>
  </button>
</template>

<script setup lang="ts">
interface Props {
  text: string
}

const props = defineProps<Props>()
const copied = ref(false)

const handleCopy = async () => {
  try {
    await navigator.clipboard.writeText(props.text)
    copied.value = true
    setTimeout(() => {
      copied.value = false
    }, 2000)
  } catch (err) {
    // Failed to copy
  }
}
</script>

