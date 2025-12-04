<template>
  <div class="min-h-screen pt-24 pb-12">
    <div class="px-4 sm:px-6 lg:px-8 ">
      <!-- Back Link -->
      <div class="mb-8">
        <NuxtLink
          to="/playground"
          class="inline-flex items-center gap-2 text-muted-foreground hover:text-foreground transition-colors"
        >
          <Icon name="heroicons:arrow-left" class="w-4 h-4" />
          Back to Playground
        </NuxtLink>
      </div>

      <!-- Loading State -->
      <div v-if="pending" class="text-center py-12">
        <Icon name="heroicons:arrow-path" class="w-8 h-8 text-muted-foreground animate-spin mx-auto mb-4" />
        <p class="text-muted-foreground">Loading playground item...</p>
      </div>

      <!-- Playground Content -->
      <div v-else-if="playground" class="space-y-8">
        <!-- Header -->
        <div>
          <div class="flex items-center gap-3 mb-4">
            <span
              :class="[
                'px-3 py-1 text-sm font-medium rounded-full',
                playground.isVueComponent ? 'bg-primary/20 text-primary' : 'bg-secondary/20 text-blue-500'
              ]"
            >
              {{ playground.codeType }}
            </span>
            <span v-if="playground.isInteractive" class="px-3 py-1 text-sm font-medium rounded-full bg-success/20 text-success">
              Interactive
            </span>
            <span v-if="playground.requiresSubscription" class="px-3 py-1 text-sm font-medium rounded-full bg-amber-500/20 text-amber-500 flex items-center gap-1">
              <Icon name="heroicons:lock-closed" class="w-4 h-4" />
              Premium
            </span>
          </div>
          <h1 class="text-4xl md:text-5xl font-bold mb-4">{{ playground.title }}</h1>
          <p class="text-lg text-muted-foreground mb-6">{{ playground.description }}</p>
          <div class="flex items-center gap-6 text-sm text-muted-foreground">
            <span class="flex items-center gap-2">
              <Icon name="heroicons:eye" class="w-5 h-5" />
              {{ playground.viewCount }} views
            </span>
            <span class="flex items-center gap-2">
              <Icon name="heroicons:heart" class="w-5 h-5" />
              {{ playground.likeCount }} likes
            </span>
          </div>
        </div>

        <!-- Live Preview Section -->
        <div class="bg-card/80 backdrop-blur-xl rounded-2xl border-2 border-border p-6 relative">
          <div class="flex items-center justify-between mb-4">
            <h2 class="text-xl font-semibold text-foreground">Live Preview</h2>
            <UiCopyButton v-if="!playground.isLocked" :text="getFullCode()" />
          </div>
          <div class="bg-muted rounded-lg p-8 min-h-[400px] flex items-center justify-center relative">
            <!-- Lock Overlay for Premium Content -->
            <div v-if="playground.isLocked" class="absolute inset-0 bg-background/80 backdrop-blur-sm rounded-lg flex flex-col items-center justify-center z-10 p-8">
              <Icon name="heroicons:lock-closed" class="w-16 h-16 text-amber-500 mb-4" />
              <h3 class="text-2xl font-bold text-foreground mb-2">Premium Content</h3>
              <p class="text-muted-foreground text-center mb-6 max-w-md">
                This playground requires a subscription to view the full code and preview. Subscribe to unlock all premium content!
              </p>
              <NuxtLink
                to="/subscribe"
                class="inline-flex items-center gap-2 px-6 py-3 bg-primary text-white rounded-lg hover:bg-primary/90 transition-colors font-medium"
              >
                <Icon name="heroicons:sparkles" class="w-5 h-5" />
                Subscribe Now
              </NuxtLink>
            </div>
            <!-- Vue Component Preview -->
            <div v-if="playground.isVueComponent && !playground.isLocked" class="w-full">
              <component :is="dynamicComponent" v-if="dynamicComponent" />
              <div v-else class="text-center text-muted-foreground">
                <Icon name="heroicons:exclamation-triangle" class="w-8 h-8 mx-auto mb-2" />
                <p>Component preview unavailable</p>
              </div>
            </div>
            <!-- HTML/CSS/JS Preview -->
            <div v-else-if="!playground.isLocked" class="w-full">
              <iframe
                :srcdoc="getPreviewHtml()"
                class="w-full h-[400px] border-0 rounded-lg"
                sandbox="allow-scripts allow-same-origin"
              />
            </div>
          </div>
        </div>

        <!-- Code Section -->
        <div class="bg-card/80 backdrop-blur-xl rounded-2xl border-2 border-border p-6 relative">
          <div class="flex items-center justify-between mb-4">
            <h2 class="text-xl font-semibold text-foreground">Code</h2>
            <UiCopyButton v-if="!playground.isLocked && playground.code" :text="playground.code" />
          </div>
          <div class="relative">
            <pre v-if="playground.isLocked" class="bg-muted rounded-lg p-4 overflow-x-auto blur-sm select-none"><code class="text-sm font-mono">// Premium content - Subscribe to unlock</code></pre>
            <pre v-else class="bg-muted rounded-lg p-4 overflow-x-auto"><code class="text-sm font-mono">{{ playground.code || 'No code available' }}</code></pre>
            <div v-if="playground.isLocked" class="absolute inset-0 flex items-center justify-center">
              <div class="bg-background/90 backdrop-blur-sm rounded-lg p-6 text-center border-2 border-amber-500/20">
                <Icon name="heroicons:lock-closed" class="w-12 h-12 text-amber-500 mx-auto mb-3" />
                <p class="text-sm text-muted-foreground mb-4">Subscribe to view the full code</p>
                <NuxtLink
                  to="/subscribe"
                  class="inline-flex items-center gap-2 px-4 py-2 bg-primary text-white rounded-lg hover:bg-primary/90 transition-colors text-sm"
                >
                  Unlock Code
                </NuxtLink>
              </div>
            </div>
          </div>
        </div>

        <!-- Additional Code Sections -->
        <div v-if="playground.css && !playground.isLocked" class="bg-card/80 backdrop-blur-xl rounded-2xl border-2 border-border p-6">
          <div class="flex items-center justify-between mb-4">
            <h2 class="text-xl font-semibold text-foreground">CSS</h2>
            <UiCopyButton :text="playground.css" />
          </div>
          <pre class="bg-muted rounded-lg p-4 overflow-x-auto"><code class="text-sm font-mono">{{ playground.css }}</code></pre>
        </div>

        <div v-if="playground.javascript && !playground.isLocked" class="bg-card/80 backdrop-blur-xl rounded-2xl border-2 border-border p-6">
          <div class="flex items-center justify-between mb-4">
            <h2 class="text-xl font-semibold text-foreground">JavaScript</h2>
            <UiCopyButton :text="playground.javascript" />
          </div>
          <pre class="bg-muted rounded-lg p-4 overflow-x-auto"><code class="text-sm font-mono">{{ playground.javascript }}</code></pre>
        </div>

        <!-- Tags -->
        <div v-if="playground.tags && playground.tags.length > 0" class="flex flex-wrap gap-2">
          <span
            v-for="tag in playground.tags"
            :key="tag"
            class="px-3 py-1 bg-primary/20 text-primary text-sm rounded-full"
          >
            #{{ tag }}
          </span>
        </div>
      </div>

      <!-- Not Found -->
      <div v-else class="text-center py-12">
        <Icon name="heroicons:exclamation-triangle" class="w-16 h-16 text-muted-foreground mx-auto mb-4" />
        <h2 class="text-2xl font-bold mb-2">Playground item not found</h2>
        <p class="text-muted-foreground mb-6">The playground item you're looking for doesn't exist.</p>
        <NuxtLink
          to="/playground"
          class="inline-flex items-center gap-2 px-4 py-2 bg-primary text-white rounded-lg hover:bg-primary/90 transition-colors"
        >
          Back to Playground
        </NuxtLink>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import type { PlaygroundDetail } from '~/../../shared/types'

const route = useRoute()
const config = useRuntimeConfig()
const apiBase = config.public.apiBase
const slug = route.params.slug as string

// Fetch playground item
const { data: playground, pending } = await useFetch<PlaygroundDetail>(
  `${apiBase}/api/playground/${slug}`,
  {
    default: () => undefined
  }
)

useHead({
  title: playground.value ? `${playground.value.title} | Playground` : 'Playground | Judah Sullivan',
})

// Dynamic component for Vue components
const dynamicComponent = computed(() => {
  if (!playground.value?.isVueComponent || !playground.value?.code) {
    return null
  }

  try {
    // For Vue components, we'll need to compile them
    // This is a simplified version - in production you might want to use a more robust solution
    const componentCode = playground.value.code
    // Note: Direct eval/compile of user code is a security risk
    // In production, consider using a sandboxed environment or pre-compiled components
    return null // Placeholder - will need proper Vue component compilation
  } catch (err) {
    console.error('Error compiling component:', err)
    return null
  }
})

// Get preview HTML for non-Vue components
const getPreviewHtml = (): string => {
  if (!playground.value) return ''
  
  const html = playground.value.code || ''
  const css = playground.value.css || ''
  const js = playground.value.javascript || ''
  
  // Build HTML string - use String.fromCharCode to avoid Vue parser issues
  const scriptOpen = '<' + String.fromCharCode(115, 99, 114, 105, 112, 116) + '>'
  const scriptClose = '</' + String.fromCharCode(115, 99, 114, 105, 112, 116) + '>'
  
  return '<!DOCTYPE html><html><head><style>' + css + '</style></head><body>' + html + scriptOpen + js + scriptClose + '</body></html>'
}

// Get full code for copying
const getFullCode = (): string => {
  if (!playground.value) return ''
  
  let code = playground.value.code || ''
  if (playground.value.css) {
    code += '\n\n/* CSS */\n' + playground.value.css
  }
  if (playground.value.javascript) {
    code += '\n\n/* JavaScript */\n' + playground.value.javascript
  }
  return code
}
</script>

