<template>
  <div class="min-h-screen pt-24 pb-12">
    <div class="px-4 sm:px-6 lg:px-8 ">
      <!-- Header -->
      <div class="mb-12">
        <UiTitle as="h1" size="4xl" weight="bold" align="left" class="text-3xl sm:text-4xl mb-4">
          Projects
        </UiTitle>
        <p class="text-base sm:text-lg text-muted-foreground max-w-2xl leading-relaxed">
          A collection of my work, from web applications to design systems and everything in between.
        </p>
      </div>

      <!-- Loading State -->
      <div v-if="pending" class="text-center py-12">
        <Icon name="heroicons:arrow-path" class="w-8 h-8 text-muted-foreground animate-spin mx-auto mb-4" />
        <p class="text-muted-foreground">Loading projects...</p>
      </div>

      <!-- Error State -->
      <div v-else-if="error" class="text-center py-12">
        <Icon name="heroicons:exclamation-triangle" class="w-16 h-16 text-danger mx-auto mb-4" />
        <p class="text-foreground font-semibold mb-2">Failed to load projects</p>
        <p class="text-sm text-muted-foreground">{{ error.message || 'Check console for details' }}</p>
        <p class="text-xs text-muted-foreground mt-2">API: {{ apiBase }}/api/projects</p>
      </div>

      <!-- Projects Grid -->
      <div v-else-if="projects && projects.length > 0" class="space-y-6">
        <!-- Featured Projects -->
        <div v-if="featuredProjects && featuredProjects.length > 0" class="mb-8">
          <h2 class="text-2xl font-bold text-foreground mb-6">Featured</h2>
          <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4 sm:gap-6">
            <NuxtLink
              v-for="(project, index) in featuredProjects"
              :key="project.id"
              :to="`/projects/${project.slug}`"
              class="group"
            >
              <UiBentoBox
                :size="'lg'"
                :variant="getVariant(index)"
                :hover="true"
                :className="'flex-col items-start justify-between h-full'"
              >
                <div class="w-full">
                  <div v-if="project.imageUrl" class="mb-4 rounded-lg overflow-hidden aspect-video bg-muted">
                    <img
                      :src="getImageUrl(project.imageUrl)"
                      :alt="project.title"
                      class="w-full h-full object-cover group-hover:scale-105 transition-transform duration-300"
                    />
                  </div>
                  <div v-else class="mb-4 rounded-lg aspect-video bg-muted flex items-center justify-center">
                    <Icon name="heroicons:folder" class="w-12 h-12 text-muted-foreground" />
                  </div>
                  <div class="flex items-center gap-2 mb-3">
                    <span class="px-2 py-1 text-xs font-medium rounded-full bg-primary/20 text-primary">
                      {{ project.category }}
                    </span>
                    <span class="px-2 py-1 text-xs font-medium rounded-full bg-secondary/20 text-blue-500">
                      {{ project.year }}
                    </span>
                    <span v-if="project.isFeatured" class="px-2 py-1 text-xs font-medium rounded-full bg-amber-500/20 text-amber-500">
                      Featured
                    </span>
                  </div>
                  <h3 class="text-xl font-bold text-foreground mb-2 group-hover:text-primary transition-colors line-clamp-2">
                    {{ project.title }}
                  </h3>
                  <p class="text-sm text-muted-foreground line-clamp-3">
                    {{ project.description }}
                  </p>
                </div>
              </UiBentoBox>
            </NuxtLink>
          </div>
        </div>

        <!-- All Projects -->
        <div>
          <h2 class="text-2xl font-bold text-foreground mb-6">All Projects</h2>
          <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4 sm:gap-6">
            <NuxtLink
              v-for="(project, index) in regularProjects"
              :key="project.id"
              :to="`/projects/${project.slug}`"
              class="group"
            >
              <UiBentoBox
                :size="'md'"
                :variant="getVariant(index)"
                :hover="true"
                :className="'flex-col items-start justify-between h-full'"
              >
                <div class="w-full">
                  <div v-if="project.imageUrl" class="mb-3 rounded-lg overflow-hidden aspect-video bg-muted">
                    <img
                      :src="getImageUrl(project.imageUrl)"
                      :alt="project.title"
                      class="w-full h-full object-cover group-hover:scale-105 transition-transform duration-300"
                    />
                  </div>
                  <div v-else class="mb-3 rounded-lg aspect-video bg-muted flex items-center justify-center">
                    <Icon name="heroicons:folder" class="w-10 h-10 text-muted-foreground" />
                  </div>
                  <div class="flex items-center gap-2 mb-2">
                    <span class="px-2 py-1 text-xs font-medium rounded-full bg-secondary/20 text-blue-500">
                      {{ project.category }}
                    </span>
                    <span class="px-2 py-1 text-xs font-medium rounded-full bg-muted text-foreground">
                      {{ project.year }}
                    </span>
                  </div>
                  <h3 class="text-lg font-bold text-foreground mb-2 group-hover:text-primary transition-colors line-clamp-2">
                    {{ project.title }}
                  </h3>
                  <p class="text-sm text-muted-foreground line-clamp-2">
                    {{ project.description }}
                  </p>
                </div>
              </UiBentoBox>
            </NuxtLink>
          </div>
        </div>
      </div>

      <!-- Empty State -->
      <div v-else class="text-center py-12">
        <Icon name="heroicons:folder" class="w-16 h-16 text-muted-foreground mx-auto mb-4" />
        <h2 class="text-2xl font-bold mb-2">No projects yet</h2>
        <p class="text-muted-foreground">Check back soon for new projects!</p>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import type { Project } from '~/../../shared/types'

const config = useRuntimeConfig()
const apiBase = config.public.apiBase

useHead({
  title: 'Projects | Judah Sullivan',
})

// Fetch projects
const { data: projects, pending, error } = await useFetch<Project[]>(
  `${apiBase}/api/projects`,
  {
    default: () => [],
    onResponse({ response }) {
      console.log('Projects API Response:', {
        status: response.status,
        data: response._data,
        count: Array.isArray(response._data) ? response._data.length : 0
      })
    },
    onResponseError({ response }) {
      console.error('Projects API Error:', {
        status: response.status,
        error: response._data
      })
    }
  }
)

// Log projects data
watch(projects, (newProjects) => {
  console.log('Projects data:', {
    count: newProjects?.length || 0,
    projects: newProjects
  })
}, { immediate: true })

// Separate featured and regular projects
const featuredProjects = computed(() => {
  return projects.value?.filter(project => project.isFeatured) || []
})

const regularProjects = computed(() => {
  return projects.value?.filter(project => !project.isFeatured) || []
})

// Get full image URL
const getImageUrl = (url?: string | null): string => {
  if (!url) return ''
  if (url.startsWith('http://') || url.startsWith('https://')) {
    return url
  }
  return `${apiBase}${url}`
}

// Get variant based on index to cycle through different colors
const getVariant = (index: number): 'primary' | 'secondary' | 'success' | 'warning' | 'danger' | 'info' | 'default' => {
  const variants: Array<'primary' | 'secondary' | 'success' | 'warning' | 'danger' | 'info' | 'default'> = [
    'primary',
    'secondary',
    'success',
    'info',
    'warning',
    'default'
  ]
  return variants[index % variants.length] || 'default'
}
</script>
