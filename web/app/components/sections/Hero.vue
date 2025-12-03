<template>
  <div class="min-h-screen mt-10 pt-10 pb-24 bg-background">
    <section>
      <div class=" px-4 sm:px-6">
        <div class="grid gap-6 items-center">
          <div class="bg-card/80 backdrop-blur-xl rounded-2xl p-6 sm:p-8 border-2 border-border shadow-lg">
            <!-- Small badge, similar subtle styling -->
            <div
              class="inline-flex items-center gap-2 rounded-full border border-border/50 bg-card/40 px-3 py-1 text-xs font-medium text-muted-foreground mb-4 sm:mb-5">
              <span class="w-1.5 h-1.5 rounded-full bg-primary animate-pulse shrink-0" />
              <span>Building thoughtful web experiences</span>
            </div>

            <UiTitle as="h1" size="5xl" weight="bold" align="left" class="mb-6 leading-tight text-3xl sm:text-4xl md:text-5xl">
              Crafting modern
              high-quality digital products.
            </UiTitle>

            <p class="text-base sm:text-lg md:text-xl text-muted-foreground max-w-xl leading-relaxed mb-8">
              I'm Judah, a full‑stack developer focused on clean design,
              performant code, and experiences that feel polished from first
              click to final deploy.
            </p>
            <!-- CTA Buttons -->
            <div class="flex flex-col md:flex-row w-full md:justify-between md:items-center gap-6">
              <div class='flex flex-wrap items-center gap-3 sm:gap-4'>
                <UiButton to="/contact" variant="primary" size="lg" class="flex items-center gap-2 text-sm sm:text-base">
                  Work with me
                  <span aria-hidden="true">→</span>
                </UiButton>

                <UiButton to="/projects" variant="outline" size="lg" class="border-border/60 bg-background/40 text-sm sm:text-base">
                  View projects
                </UiButton>
              </div>

              <div class="flex items-center gap-3 shrink-0">
                <span class="w-2.5 h-2.5 rounded-full bg-success animate-pulse shrink-0" />
                <span class="text-xs sm:text-sm text-muted-foreground">
                  Currently open for freelance work & collaborations
                </span>
              </div>
            </div>

            <!-- Availability pill (mirrors contact page “open to opportunities”) -->

          </div>

        </div>
      </div>
    </section>
  </div>
</template>

<script setup lang="ts">
// Hero section component
</script>
