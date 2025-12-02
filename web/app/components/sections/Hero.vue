<template>
  <div class="min-h-screen pt-10 pb-24">
    <section>
      <div class=" px-4 sm:px-6">
        <div class="grid gap-6 items-center">
          <div class="bg-card/50 backdrop-blur-xl rounded-2xl p-6 sm:p-8 border border-border/50 shadow-sm">
            <!-- Small badge, similar subtle styling -->
            <div
              class="inline-flex items-center gap-2 rounded-full border border-border/50 bg-card/40 px-3 py-1 text-xs font-medium text-muted-foreground mb-5">
              <span class="w-1.5 h-1.5 rounded-full bg-primary animate-pulse" />
              <span>Building thoughtful web experiences</span>
            </div>

            <UiTitle as="h1" size="5xl" weight="bold" align="left" class="mb-6 leading-[.5]">
              Crafting modern
              high-quality digital products.
            </UiTitle>

            <p class="text-lg sm:text-xl text-muted-foreground max-w-xl leading-relaxed mb-8">
              I’m Judah, a full‑stack developer focused on clean design,
              performant code, and experiences that feel polished from first
              click to final deploy.
            </p>
            <!-- CTA Buttons -->
            <div class="flex w-full justify-between items-center ">
              <div class='flex flex-wrap items-center gap-4'>
                <UiButton href="/contact" variant="primary" size="lg" class="flex items-center gap-2">
                  Work with me
                  <span aria-hidden="true">→</span>
                </UiButton>

                <UiButton href="/projects" variant="outline" size="lg" class="border-border/60 bg-background/40">
                  View projects
                </UiButton>
              </div>

              <div class="mt-4 flex items-center gap-3">
                <span class="w-2.5 h-2.5 rounded-full bg-success animate-pulse" />
                <span class="text-sm text-muted-foreground">
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
</script>
