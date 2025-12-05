<template>
  <section class=" min-h-screen">
    <div class="px-4 sm:px-6">
      <UiBentoBox
        container
        backdrop
        border
        shadow
        padding="lg"
        rounded="2xl"
        :className="homepageData.sections.aboutMe.background"
      >
        <UiTitle as="h2" size="4xl" weight="bold" align="left" class="mb-6">
          About Me
        </UiTitle>
        <div class="space-y-4">
          <div>
            <p class="text-lg sm:text-xl font-semibold text-foreground mb-2">
              {{ aboutData.bio.tagline }}
            </p>
            <p class="text-base sm:text-lg text-muted-foreground max-w-2xl leading-relaxed">
              {{ aboutData.bio.description }}
            </p>
          </div>
          <div class="flex flex-wrap items-center gap-4 pt-4 border-t border-border/50">
            <div class="flex items-center gap-2 text-sm text-muted-foreground">
              <Icon name="heroicons:map-pin" class="w-4 h-4" />
              <span>{{ aboutData.bio.location }}</span>
            </div>
            <div v-if="aboutData.bio.status" class="px-3 py-1 text-xs font-medium rounded-full bg-success/20 text-success">
              {{ aboutData.bio.status }}
            </div>
          </div>
          <div class="flex items-center gap-4 pt-2">
            <a 
              :href="aboutData.bio.github" 
              target="_blank" 
              rel="noopener noreferrer"
              class="text-sm text-primary hover:underline flex items-center gap-2"
            >
              <Icon name="heroicons:code-bracket-square" class="w-4 h-4" />
              GitHub
            </a>
            <a 
              v-if="aboutData.bio.website"
              :href="aboutData.bio.website" 
              target="_blank" 
              rel="noopener noreferrer"
              class="text-sm text-primary hover:underline flex items-center gap-2"
            >
              <Icon name="heroicons:globe-alt" class="w-4 h-4" />
              Website
            </a>
            <NuxtLink
              to="/about"
              class="text-sm text-primary hover:underline flex items-center gap-2"
            >
              Learn more
              <Icon name="heroicons:arrow-right" class="w-4 h-4" />
            </NuxtLink>
          </div>
        </div>
      </UiBentoBox>
    </div>
  </section>
</template>

<script setup lang="ts">
import homepageData from '../../data/homepage.json'
import aboutData from '../../data/about.json'
// About Me component
</script>

