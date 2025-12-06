<template>
  <div class="min-h-screen pt-24 pb-12">
    <div class="px-4 sm:px-6 lg:px-8 ">
      <!-- Header -->
      <div class="mb-12">
        <UiTitle as="h1" size="4xl" weight="bold" align="left" class="text-3xl sm:text-4xl mb-4">
          Blog
        </UiTitle>
        <p class="text-base sm:text-lg text-muted-foreground max-w-2xl leading-relaxed">
          Thoughts, tutorials, and insights on web development, design, and technology.
        </p>
      </div>

      <!-- Loading State -->
      <div v-if="pending" class="text-center py-12">
        <Icon name="heroicons:arrow-path" class="w-8 h-8 text-muted-foreground animate-spin mx-auto mb-4" />
        <p class="text-muted-foreground">Loading blog posts...</p>
      </div>

      <!-- Error State -->
      <div v-else-if="error" class="text-center py-12">
        <Icon name="heroicons:exclamation-triangle" class="w-16 h-16 text-danger mx-auto mb-4" />
          <p class="text-muted-foreground">No Blogs Articles yet. Check back soon!</p>
      </div>

      <!-- Blog Posts Grid -->
      <div v-else-if="posts && posts.length > 0" class="space-y-6">
        <!-- Featured Post (if any) -->
        <div v-if="featuredPosts && featuredPosts.length > 0" class="mb-8">
          <h2 class="text-2xl font-bold text-foreground mb-6">Featured</h2>
          <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4 sm:gap-6">
            <NuxtLink
              v-for="(post, index) in featuredPosts"
              :key="post.id"
              :to="`/blog/${post.slug}`"
              class="group"
            >
              <UiBentoBox
                :size="'lg'"
                :variant="getVariant(index)"
                :hover="true"
                :className="'flex-col items-start justify-between h-full'"
              >
                <div class="w-full">
                  <div v-if="post.image" class="mb-4 rounded-lg overflow-hidden aspect-video bg-muted">
                    <img
                      :src="getImageUrl(post.image)"
                      :alt="post.title"
                      class="w-full h-full object-cover group-hover:scale-105 transition-transform duration-300"
                    />
                  </div>
                  <div class="flex items-center gap-2 mb-3">
                    <span class="px-2 py-1 text-xs font-medium rounded-full bg-primary/20 text-primary">
                      {{ post.category }}
                    </span>
                    <span v-if="post.isFeatured" class="px-2 py-1 text-xs font-medium rounded-full bg-amber-500/20 text-amber-500">
                      Featured
                    </span>
                  </div>
                  <h3 class="text-xl font-bold text-foreground mb-2 group-hover:text-primary transition-colors line-clamp-2">
                    {{ post.title }}
                  </h3>
                  <p class="text-sm text-muted-foreground line-clamp-3 mb-4">
                    {{ post.excerpt }}
                  </p>
                </div>
                <div class="flex items-center justify-between w-full text-xs text-muted-foreground">
                  <span class="flex items-center gap-1">
                    <Icon name="heroicons:clock" class="w-4 h-4" />
                    {{ post.readTimeMinutes }} min read
                  </span>
                  <span v-if="post.publishedAt" class="flex items-center gap-1">
                    <Icon name="heroicons:calendar" class="w-4 h-4" />
                    {{ formatDate(post.publishedAt) }}
                  </span>
                </div>
              </UiBentoBox>
            </NuxtLink>
          </div>
        </div>

        <!-- All Posts -->
        <div>
          <h2 class="text-2xl font-bold text-foreground mb-6">All Posts</h2>
          <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4 sm:gap-6">
            <NuxtLink
              v-for="(post, index) in regularPosts"
              :key="post.id"
              :to="`/blog/${post.slug}`"
              class="group"
            >
              <UiBentoBox
                :size="'md'"
                :variant="getVariant(index)"
                :hover="true"
                :className="'flex-col items-start justify-between h-full'"
              >
                <div class="w-full">
                  <div v-if="post.image" class="mb-3 rounded-lg overflow-hidden aspect-video bg-muted">
                    <img
                      :src="getImageUrl(post.image)"
                      :alt="post.title"
                      class="w-full h-full object-cover group-hover:scale-105 transition-transform duration-300"
                    />
                  </div>
                  <div class="flex items-center gap-2 mb-2">
                    <span class="px-2 py-1 text-xs font-medium rounded-full bg-secondary/20 text-blue-500">
                      {{ post.category }}
                    </span>
                  </div>
                  <h3 class="text-lg font-bold text-foreground mb-2 group-hover:text-primary transition-colors line-clamp-2">
                    {{ post.title }}
                  </h3>
                  <p class="text-sm text-muted-foreground line-clamp-2 mb-3">
                    {{ post.excerpt }}
                  </p>
                </div>
                <div class="flex items-center justify-between w-full text-xs text-muted-foreground">
                  <span class="flex items-center gap-1">
                    <Icon name="heroicons:clock" class="w-4 h-4" />
                    {{ post.readTimeMinutes }} min
                  </span>
                  <span v-if="post.publishedAt" class="flex items-center gap-1">
                    <Icon name="heroicons:calendar" class="w-4 h-4" />
                    {{ formatDate(post.publishedAt) }}
                  </span>
                </div>
              </UiBentoBox>
            </NuxtLink>
          </div>
        </div>
      </div>

      <!-- Empty State -->
      <div v-else class="text-center py-12">
        <Icon name="heroicons:document-text" class="w-16 h-16 text-muted-foreground mx-auto mb-4" />
        <h2 class="text-2xl font-bold mb-2">No blog posts yet</h2>
        <p class="text-muted-foreground">Check back soon for new content!</p>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import type { BlogPost } from '~/../../shared/types'

const config = useRuntimeConfig()
const apiBase = config.public.apiBase

useHead({
  title: 'Blog | Judah Sullivan',
})

// Fetch blog posts
const { data: posts, pending, error } = await useFetch<BlogPost[]>(
  `${apiBase}/api/blog`,
  {
    default: () => []
  }
)

// Separate featured and regular posts
const featuredPosts = computed(() => {
  return posts.value?.filter(post => post.isFeatured) || []
})

const regularPosts = computed(() => {
  return posts.value?.filter(post => !post.isFeatured) || []
})

// Get full image URL
const getImageUrl = (url?: string | null): string => {
  if (!url) return ''
  if (url.startsWith('http://') || url.startsWith('https://')) {
    return url
  }
  return `${apiBase}${url}`
}

// Format date
const formatDate = (dateString?: string): string => {
  if (!dateString) return ''
  const date = new Date(dateString)
  return date.toLocaleDateString('en-US', { year: 'numeric', month: 'short', day: 'numeric' })
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
