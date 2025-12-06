<template>
  <div class="min-h-screen pt-24">
    <!-- Back Link -->
    <div class=" px-6 py-8">
      <NuxtLink to="/blog" class="text-neutral-400 hover:text-white transition-colors">
        ← Back to Blog
      </NuxtLink>
    </div>

    <!-- Article Header -->
    <article>
      <header class="pb-12">
        <div class="container max-w-4xl mx-auto px-6">
          <span class="inline-block px-3 py-1 bg-neutral-900 rounded-full text-sm text-neutral-400 mb-6">
            {{ post.category }}
          </span>
          <h1 class="text-4xl md:text-5xl font-bold mb-6 leading-tight">
            {{ post.title }}
          </h1>
          <div class="flex items-center gap-6 text-neutral-400">
            <div class="flex items-center gap-3">
              <div class="w-10 h-10 bg-neutral-800 rounded-full" />
              <span>Judah Sullivan</span>
            </div>
            <span>{{ post.date }}</span>
            <span>{{ post.readTime }} min read</span>
          </div>
        </div>
      </header>

      <!-- Featured Image -->
      <div class="container max-w-5xl mx-auto px-6 pb-12">
        <div class="aspect-video bg-neutral-900 rounded-2xl" />
      </div>

      <!-- Article Content -->
      <div class="container max-w-4xl mx-auto px-6 pb-20">
        <div class="prose prose-lg prose-invert max-w-none">
          <p class="text-xl text-neutral-300 leading-relaxed mb-8">
            {{ post.excerpt }}
          </p>

          <h2>Introduction</h2>
          <p>
            Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod 
            tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, 
            quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.
          </p>

          <h2>Getting Started</h2>
          <p>
            Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore 
            eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt 
            in culpa qui officia deserunt mollit anim id est laborum.
          </p>

          <pre><code>// Example code block
const example = () => {
  // Your code here
}
</code></pre>

          <h2>Key Concepts</h2>
          <p>
            Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium 
            doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore 
            veritatis et quasi architecto beatae vitae dicta sunt explicabo.
          </p>

          <ul>
            <li>First important point about this topic</li>
            <li>Second key concept to understand</li>
            <li>Third thing you should know</li>
            <li>Final consideration</li>
          </ul>

          <h2>Conclusion</h2>
          <p>
            Nemo enim ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit, 
            sed quia consequuntur magni dolores eos qui ratione voluptatem sequi nesciunt.
          </p>
        </div>
      </div>

      <!-- Tags -->
      <div class="container max-w-4xl mx-auto px-6 pb-12">
        <div class="flex flex-wrap gap-2">
          <span
            v-for="tag in post.tags"
            :key="tag"
            class="px-3 py-1 bg-neutral-900 rounded-full text-sm text-neutral-400"
          >
            #{{ tag }}
          </span>
        </div>
      </div>
    </article>

    <!-- Author Bio -->
    <section class="py-12 border-t border-neutral-800">
      <div class="container max-w-4xl mx-auto px-6">
        <div class="flex items-start gap-6">
          <div class="w-16 h-16 bg-neutral-800 rounded-full shrink-0" />
          <div>
            <h3 class="text-lg font-semibold mb-2">Written by Judah Sullivan</h3>
            <p class="text-neutral-400 mb-4">
              Full-stack developer passionate about Vue, .NET, and creating 
              interactive web experiences.
            </p>
            <NuxtLink to="/about" class="text-blue-400 hover:text-blue-300 transition-colors">
              More about me →
            </NuxtLink>
          </div>
        </div>
      </div>
    </section>

    <!-- Related Posts -->
    <section class="py-20 bg-neutral-950">
      <div class="container max-w-6xl mx-auto px-6">
        <h2 class="text-2xl font-bold mb-12">Related Articles</h2>
        <div class="grid md:grid-cols-3 gap-8">
          <NuxtLink
            v-for="related in relatedPosts"
            :key="related.slug"
            :to="`/blog/${related.slug}`"
            class="group"
          >
            <div class="aspect-video bg-neutral-900 rounded-xl mb-4" />
            <h3 class="font-semibold group-hover:text-neutral-300 transition-colors">
              {{ related.title }}
            </h3>
            <p class="text-neutral-500 text-sm mt-2">{{ related.date }}</p>
          </NuxtLink>
        </div>
      </div>
    </section>
  </div>
</template>

<script setup lang="ts">
const route = useRoute()
const slug = route.params.slug as string

// Mock data - will be replaced with API call
const post = ref({
  slug: slug,
  title: 'Getting Started with Nuxt 4',
  category: 'Vue/Nuxt',
  excerpt: 'An introduction to the latest version of Nuxt and all the exciting new features it brings to the table for modern web development.',
  date: 'Nov 28, 2024',
  readTime: 8,
  tags: ['nuxt', 'vue', 'javascript', 'web-development'],
})

const relatedPosts = ref([
  { slug: 'vue-composables-patterns', title: 'Vue 3 Composables: Best Practices', date: 'Nov 10, 2024' },
  { slug: 'gsap-animation-techniques', title: 'Advanced GSAP Animation Techniques', date: 'Nov 25, 2024' },
  { slug: 'building-apis-dotnet', title: 'Building Robust APIs with .NET', date: 'Nov 20, 2024' },
])

useHead({
  title: `${post.value.title} | Blog`,
})
</script>


