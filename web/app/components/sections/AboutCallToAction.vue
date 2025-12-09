<template>
  <UiBentoBox
    container
    backdrop
    border
    shadow
    padding="xl"
    rounded="2xl"
    :className="`text-center ${ctaData.background}`"
  >
    <div class="max-w-2xl mx-auto">
      <Icon :name="ctaData.icon" class="w-12 h-12 text-primary mx-auto mb-4" />
      <h3 class="text-2xl font-bold text-foreground mb-4">{{ ctaData.title }}</h3>
      <p class="text-base text-muted-foreground mb-6">
        {{ ctaData.description }}
      </p>
      <div class="flex flex-col sm:flex-row gap-4 justify-center">
        <UiButton :to="ctaData.primaryButton.to" variant="primary" size="lg">
          <Icon :name="ctaData.primaryButton.icon" class="w-5 h-5" />
          {{ ctaData.primaryButton.text }}
        </UiButton>
        <UiButton :to="ctaData.secondaryButton.to" variant="outline" size="lg">
          <Icon :name="ctaData.secondaryButton.icon" class="w-5 h-5" />
          {{ ctaData.secondaryButton.text }}
        </UiButton>
      </div>
    </div>
  </UiBentoBox>
</template>

<script setup lang="ts">
interface Props {
  ctaData: {
    icon: string
    title: string
    description: string
    background: string
    primaryButton: {
      text: string
      to: string
      icon: string
    }
    secondaryButton: {
      text: string
      to: string
      icon: string
    }
  }
}

defineProps<Props>()
</script>


