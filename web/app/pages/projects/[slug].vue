<template>
  <div class="min-h-screen pt-24">
    <!-- Loading -->
    <div v-if="pending" class="container max-w-6xl mx-auto py-20">
      <div class="animate-pulse space-y-8">
        <div class="h-4 bg-gray-800 rounded w-32" />
        <div class="h-12 bg-gray-800 rounded w-2/3" />
        <div class="h-6 bg-gray-800 rounded w-1/2" />
        <div class="aspect-video bg-gray-800 rounded-2xl" />
      </div>
    </div>

    <!-- Not Found -->
    <div v-else-if="!project" class="container max-w-6xl mx-auto py-20 text-center">
      <h1 class="text-4xl font-bold mb-4">Project Not Found</h1>
      <p class="text-muted-foreground mb-8">The project you're looking for doesn't exist.</p>
      <NuxtLink to="/projects" class="btn-primary">
        ← Back to Projects
      </NuxtLink>
    </div>

    <!-- Project Content -->
    <template v-else>
      <!-- Back Link -->
      <div class="container max-w-6xl mx-auto py-8">
        <NuxtLink to="/projects" class="inline-flex items-center gap-2 text-muted-foreground hover:text-white transition-colors">
          <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 19l-7-7 7-7" />
          </svg>
          Back to Projects
        </NuxtLink>
      </div>

      <!-- Project Header -->
      <section class="pb-16">
        <div class="container max-w-6xl mx-auto">
          <div class="grid md:grid-cols-2 gap-12 items-end">
            <div>
              <span class="tag mb-4" :class="categoryTagClass">
                {{ project.category }}
              </span>
              <h1 class="text-4xl md:text-5xl lg:text-6xl font-bold mb-6 tracking-tight">
                {{ project.title }}
              </h1>
              <p class="text-lg text-muted-foreground">
                {{ project.description }}
              </p>
            </div>
            <div class="flex flex-col gap-6 md:items-end">
              <div class="grid grid-cols-2 gap-6">
                <div>
                  <p class="text-sm text-muted-foreground mb-1">Year</p>
                  <p class="font-semibold">{{ project.year }}</p>
                </div>
                <div>
                  <p class="text-sm text-muted-foreground mb-1">Role</p>
                  <p class="font-medium">{{ project.role }}</p>
                </div>
              </div>
              <div class="flex gap-3">
                <a
                  v-if="project.liveUrl"
                  :href="project.liveUrl"
                  target="_blank"
                  class="btn-primary flex items-center gap-2"
                >
                  <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10 6H6a2 2 0 00-2 2v10a2 2 0 002 2h10a2 2 0 002-2v-4M14 4h6m0 0v6m0-6L10 14" />
                  </svg>
                  View Live
                </a>
                <a
                  v-if="project.githubUrl"
                  :href="project.githubUrl"
                  target="_blank"
                  class="btn-secondary flex items-center gap-2"
                >
                  <svg class="w-4 h-4" fill="currentColor" viewBox="0 0 24 24">
                    <path d="M12 0C5.37 0 0 5.37 0 12c0 5.31 3.435 9.795 8.205 11.385.6.105.825-.255.825-.57 0-.285-.015-1.23-.015-2.235-3.015.555-3.795-.735-4.035-1.41-.135-.345-.72-1.41-1.23-1.695-.42-.225-1.02-.78-.015-.795.945-.015 1.62.87 1.845 1.23 1.08 1.815 2.805 1.305 3.495.99.105-.78.42-1.305.765-1.605-2.67-.3-5.46-1.335-5.46-5.925 0-1.305.465-2.385 1.23-3.225-.12-.3-.54-1.53.12-3.18 0 0 1.005-.315 3.3 1.23.96-.27 1.98-.405 3-.405s2.04.135 3 .405c2.295-1.56 3.3-1.23 3.3-1.23.66 1.65.24 2.88.12 3.18.765.84 1.23 1.905 1.23 3.225 0 4.605-2.805 5.625-5.475 5.925.435.375.81 1.095.81 2.22 0 1.605-.015 2.895-.015 3.3 0 .315.225.69.825.57A12.02 12.02 0 0024 12c0-6.63-5.37-12-12-12z" />
                  </svg>
                  GitHub
                </a>
              </div>
            </div>
          </div>
        </div>
      </section>

      <!-- Hero Image -->
      <section class="pb-20">
        <div class="container max-w-6xl mx-auto">
          <div class="grid-card overflow-hidden">
            <div class="aspect-video bg-gray-800">
              <img
                v-if="project.imageUrl"
                :src="project.imageUrl"
                :alt="project.title"
                class="w-full h-full object-cover"
              />
            </div>
          </div>
        </div>
      </section>

      <!-- Overview -->
      <section class="section section-alt">
        <div class="container max-w-4xl mx-auto">
          <h2 class="text-3xl font-bold mb-8">Overview</h2>
          <p class="text-lg text-muted-foreground leading-relaxed">
            {{ project.overview }}
          </p>
        </div>
      </section>

      <!-- Tech Stack -->
      <section v-if="project.techStack?.length" class="section">
        <div class="container max-w-6xl mx-auto">
          <h2 class="text-3xl font-bold mb-8">Tech Stack</h2>
          <div class="flex flex-wrap gap-3">
            <span
              v-for="tech in project.techStack"
              :key="tech"
              class="px-4 py-2 grid-card text-sm font-medium"
            >
              {{ tech }}
            </span>
          </div>
        </div>
      </section>

      <!-- Features -->
      <section v-if="project.features?.length" class="section section-alt">
        <div class="container max-w-6xl mx-auto">
          <h2 class="text-3xl font-bold mb-12">Key Features</h2>
          <div class="grid md:grid-cols-2 gap-6">
            <div
              v-for="(feature, index) in project.features"
              :key="index"
              class="grid-card p-6"
              :class="featureCardClass(index)"
            >
              <div class="flex items-start gap-4">
                <span class="w-8 h-8 rounded-lg flex items-center justify-center text-sm font-bold" :class="featureDotClass(index)">
                  {{ index + 1 }}
                </span>
                <div>
                  <h3 class="text-lg font-semibold mb-2">{{ feature.title }}</h3>
                  <p class="text-muted-foreground">{{ feature.description }}</p>
                </div>
              </div>
            </div>
          </div>
        </div>
      </section>

      <!-- Gallery -->
      <section v-if="project.galleryImages?.length" class="section">
        <div class="container max-w-6xl mx-auto">
          <h2 class="text-3xl font-bold mb-12">Gallery</h2>
          <div class="grid md:grid-cols-2 gap-6">
            <div
              v-for="(image, index) in project.galleryImages"
              :key="index"
              class="grid-card overflow-hidden"
            >
              <div class="aspect-video bg-gray-800">
                <img :src="image" :alt="`${project.title} screenshot ${index + 1}`" class="w-full h-full object-cover" />
              </div>
            </div>
          </div>
        </div>
      </section>
    </template>
  </div>
</template>

<script setup lang="ts">
import type { ProjectDetail } from '~/../../shared/types'

const route = useRoute()
const config = useRuntimeConfig()
const slug = route.params.slug as string

// Use unique key for caching per project - Nuxt automatically handles de-duplication
const cacheKey = `project:${slug}`

const { data: project, pending } = await useFetch<ProjectDetail | null>(
  `${config.public.apiBase}/api/projects/${slug}`,
  {
    key: cacheKey, // Unique key for caching/de-duplication across components
    watch: false, // Don't re-fetch on route changes - use cached data
    default: () => null as ProjectDetail | null,
    // Server-side caching: data fetched once during SSR and reused
  }
)

const categoryTagClass = computed(() => {
  if (!project.value) return ''
  const category = project.value.category.toLowerCase()
  if (category.includes('full')) return 'tag-blue'
  if (category.includes('front')) return 'tag-green'
  if (category.includes('api') || category.includes('back')) return 'tag-red'
  return ''
})

const featureCardClass = (index: number) => {
  const classes = ['grid-card-blue', 'grid-card-green', 'grid-card-red', '']
  return classes[index % classes.length]
}

const featureDotClass = (index: number) => {
  const classes = [
      'bg-primary/20 text-primary',
      'bg-success/20 text-success',
      'bg-destructive/20 text-destructive',
      'bg-gray-800 text-white'
  ]
  return classes[index % classes.length]
}

useHead({
  title: computed(() => project.value ? `${project.value.title} | Projects` : 'Project'),
})
</script>
