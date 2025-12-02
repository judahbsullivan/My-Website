<template>
  <section class="w-full">
    <div class="px-4 sm:px-6">
      <div class="grid grid-cols-1 lg:grid-cols-12 gap-6">
        <!-- What I do - Spans 7 columns (slightly more) -->
        <div class="lg:col-span-7">
          <div class="bg-card/50 backdrop-blur-xl rounded-2xl p-6 sm:p-8 border border-border/50 shadow-sm">
            <UiTitle as="h2" size="md" weight="semibold" class="mb-6">
              What I do
            </UiTitle>

            <div class="space-y-5">
              <div class="flex items-start gap-4">
                <div
                  class="w-9 h-9 rounded-xl bg-primary/10 text-primary flex items-center justify-center text-sm font-semibold">
                  FE
                </div>
                <div>
                  <p class="text-sm font-medium mb-1">Front‑end</p>
                  <p class="text-sm text-muted-foreground">
                    Modern, responsive interfaces with Vue/Nuxt and a focus
                    on accessibility and performance.
                  </p>
                </div>
              </div>

              <div class="flex items-start gap-4">
                <div
                  class="w-9 h-9 rounded-xl bg-primary/10 text-primary flex items-center justify-center text-sm font-semibold">
                  BE
                </div>
                <div>
                  <p class="text-sm font-medium mb-1">Back‑end</p>
                  <p class="text-sm text-muted-foreground">
                    Robust APIs with .NET and PostgreSQL, designed for
                    scalability and maintainability.
                  </p>
                </div>
              </div>

              <div class="flex items-start gap-4">
                <div
                  class="w-9 h-9 rounded-xl bg-primary/10 text-primary flex items-center justify-center text-sm font-semibold">
                  UX
                </div>
                <div>
                  <p class="text-sm font-medium mb-1">Product thinking</p>
                  <p class="text-sm text-muted-foreground">
                    Clear flows, thoughtful micro‑interactions, and an
                    emphasis on communicating value to users.
                  </p>
                </div>
              </div>
            </div>
          </div>
        </div>

        <!-- Quick facts - Spans 5 columns (slightly less) -->
        <div class="lg:col-span-5">
          <div class="bg-card/50 backdrop-blur-xl rounded-2xl p-6 sm:p-8 border border-border/50 shadow-sm">
            <UiTitle as="h3" size="sm" weight="semibold" class="mb-5">
              Quick facts
            </UiTitle>
            <div class="space-y-4">
              <div class="flex justify-between items-center py-2 border-b border-border/30">
                <span class="text-sm text-muted-foreground">Experience</span>
                <span class="text-sm font-medium">X+ years</span>
              </div>
              <div class="flex justify-between items-center py-2 border-b border-border/30">
                <span class="text-sm text-muted-foreground">Focus</span>
                <span class="text-sm font-medium">
                  Web apps, APIs, architecture
                </span>
              </div>
              <div class="flex justify-between items-center py-2">
                <span class="text-sm text-muted-foreground">Stack</span>
                <span class="text-sm font-medium">
                  Vue/Nuxt · .NET · PostgreSQL
                </span>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
// What I do and Quick facts component
</script>

