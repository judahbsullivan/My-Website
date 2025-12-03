<template>
  <!-- Creative Grid Section - Shows peek of next section -->
  <div class="relative mt-12 -mb-20">
    <div class="bg-card/50 backdrop-blur-xl rounded-3xl p-8 sm:p-10 lg:p-12 border border-border/50 shadow-lg">
      <!-- Header with Big Text and Description -->
      <div class="mb-8">
        <div class="flex items-start justify-between mb-4">
          <div class="flex-1">
            <UiTitle as="h2" size="4xl" weight="bold" align="left" class="mb-3">
              Featured Work
            </UiTitle>
            <p class="text-lg text-muted-foreground max-w-2xl leading-relaxed">
              Explore my latest projects and creative solutions
            </p>
          </div>
          <div class="hidden lg:flex items-center gap-2 text-muted-foreground ml-8">
            <span class="text-xs uppercase tracking-wider">Scroll</span>
            <svg class="w-5 h-5 animate-bounce" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 14l-7 7m0 0l-7-7m7 7V3" />
            </svg>
          </div>
        </div>
      </div>
      
      <!-- Creative Grid Layout -->
      <div class="grid grid-cols-1 md:grid-cols-3 gap-4 lg:gap-6">
        <!-- Large Featured Card -->
        <div class="md:col-span-2 aspect-video rounded-xl bg-linear-to-br from-primary/20 to-primary/5 border border-border/30 flex items-center justify-center overflow-hidden group hover:border-primary/50 transition-colors">
          <div class="text-center">
            <span class="text-muted-foreground text-sm group-hover:text-primary transition-colors">Featured Project</span>
          </div>
        </div>
        
        <!-- Small Card 1 -->
        <div class="aspect-square rounded-xl bg-linear-to-br from-primary/20 to-primary/5 border border-border/30 flex items-center justify-center overflow-hidden group hover:border-primary/50 transition-colors">
          <div class="text-center">
            <span class="text-muted-foreground text-sm group-hover:text-primary transition-colors">Project</span>
          </div>
        </div>
        
        <!-- Small Card 2 -->
        <div class="aspect-square rounded-xl bg-linear-to-br from-primary/20 to-primary/5 border border-border/30 flex items-center justify-center overflow-hidden group hover:border-primary/50 transition-colors">
          <div class="text-center">
            <span class="text-muted-foreground text-sm group-hover:text-primary transition-colors">Project</span>
          </div>
        </div>
        
        <!-- Medium Card -->
        <div class="md:col-span-2 aspect-video rounded-xl bg-linear-to-br from-primary/20 to-primary/5 border border-border/30 flex items-center justify-center overflow-hidden group hover:border-primary/50 transition-colors">
          <div class="text-center">
            <span class="text-muted-foreground text-sm group-hover:text-primary transition-colors">Project Preview</span>
          </div>
        </div>
        
        <!-- Small Card 3 -->
        <div class="aspect-square rounded-xl bg-linear-to-br from-primary/20 to-primary/5 border border-border/30 flex items-center justify-center overflow-hidden group hover:border-primary/50 transition-colors">
          <div class="text-center">
            <span class="text-muted-foreground text-sm group-hover:text-primary transition-colors">Project</span>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
// Featured Work component
</script>

