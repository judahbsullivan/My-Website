<template>
  <div class="min-h-screen pt-32 pb-20">
    <!-- Contact Content -->
    <section>
      <div class="container max-w-5xl mx-auto px-4 sm:px-6">
        <div class="mb-16 sm:mb-20">
          <UiTitle as="h1" size="5xl" weight="bold" align="left" class="mb-6">
            Get in Touch
          </UiTitle>
          <p class="text-lg sm:text-xl text-muted-foreground max-w-2xl leading-relaxed">
            Have a project in mind? Let's work together to bring your ideas to life.
          </p>
        </div>

        <div class="grid lg:grid-cols-2 gap-8 lg:gap-12">
          <!-- Contact Form -->
          <div class="bg-card/50 backdrop-blur-xl rounded-2xl p-6 sm:p-8 lg:p-10 border border-border/50 shadow-sm">
            <UiTitle as="h2" size="md" weight="semibold" class="mb-8">Send a Message</UiTitle>
            <form @submit.prevent="handleSubmit" class="space-y-6">
              <div>
                <label for="name" class="block text-sm font-medium text-muted-foreground mb-3">
                  Name
                </label>
                <input id="name" v-model="form.name" type="text" required placeholder="Your name"
                  class="w-full px-4 py-3 bg-background/50 border border-border/50 rounded-xl text-foreground placeholder:text-muted-foreground/50 focus:outline-none focus:ring-2 focus:ring-primary/20 focus:border-primary/50 transition-all" />
              </div>

              <div>
                <label for="email" class="block text-sm font-medium text-muted-foreground mb-3">
                  Email
                </label>
                <input id="email" v-model="form.email" type="email" required placeholder="you@example.com"
                  class="w-full px-4 py-3 bg-background/50 border border-border/50 rounded-xl text-foreground placeholder:text-muted-foreground/50 focus:outline-none focus:ring-2 focus:ring-primary/20 focus:border-primary/50 transition-all" />
              </div>

              <div>
                <label for="subject" class="block text-sm font-medium text-muted-foreground mb-3">
                  Subject
                </label>
                <select id="subject" v-model="form.subject" required
                  class="w-full px-4 py-3 bg-background/50 border border-border/50 rounded-xl text-foreground focus:outline-none focus:ring-2 focus:ring-primary/20 focus:border-primary/50 transition-all">
                  <option value="" disabled>Select a subject</option>
                  <option value="project">Project Inquiry</option>
                  <option value="collaboration">Collaboration</option>
                  <option value="job">Job Opportunity</option>
                  <option value="other">Other</option>
                </select>
              </div>

              <div>
                <label for="message" class="block text-sm font-medium text-muted-foreground mb-3">
                  Message
                </label>
                <textarea id="message" v-model="form.message" required rows="5"
                  placeholder="Tell me about your project..."
                  class="w-full px-4 py-3 bg-background/50 border border-border/50 rounded-xl text-foreground placeholder:text-muted-foreground/50 focus:outline-none focus:ring-2 focus:ring-primary/20 focus:border-primary/50 transition-all resize-none" />
              </div>

              <!-- Success Message -->
              <div v-if="successMessage" class="p-4 rounded-xl bg-success/10 border border-success/20">
                <div class="flex items-center gap-3">
                  <span class="w-2 h-2 rounded-full bg-success" />
                  <p class="text-success text-sm">{{ successMessage }}</p>
                </div>
              </div>

              <!-- Error Message -->
              <div v-if="errorMessage" class="p-4 rounded-xl bg-destructive/10 border border-destructive/20">
                <div class="flex items-center gap-3">
                  <span class="w-2 h-2 rounded-full bg-destructive" />
                  <p class="text-destructive text-sm">{{ errorMessage }}</p>
                </div>
              </div>

              <UiButton type="submit" variant="primary" size="lg" :disabled="isSubmitting" class="w-full mt-2">
                <svg v-if="isSubmitting" class="animate-spin w-5 h-5" fill="none" viewBox="0 0 24 24">
                  <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4" />
                  <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4z" />
                </svg>
                <span v-if="!isSubmitting">Send Message</span>
                <span v-else>Sending...</span>
              </UiButton>
            </form>
          </div>

          <!-- Contact Info -->
          <div class="space-y-6">
            <div class="bg-card/50 backdrop-blur-xl rounded-2xl p-6 sm:p-8 border border-border/50 shadow-sm">
              <UiTitle as="h3" size="md" weight="semibold" class="mb-6">Contact Information</UiTitle>

              <div class="space-y-6">
                <div>
                  <p class="text-sm text-muted-foreground mb-2">Email</p>
                  <a href="mailto:hello@judahsullivan.com"
                    class="text-lg font-medium hover:text-primary transition-colors">
                    hello@judahsullivan.com
                  </a>
                </div>

                <div>
                  <p class="text-sm text-muted-foreground mb-4">Social</p>
                  <div class="flex gap-3">
                    <UiButton href="https://github.com/judahbsullivan" target="_blank" variant="primary" size="sm"
                      class="w-11! h-11! p-0!">
                      <svg class="w-5 h-5" fill="currentColor" viewBox="0 0 24 24">
                        <path
                          d="M12 0C5.37 0 0 5.37 0 12c0 5.31 3.435 9.795 8.205 11.385.6.105.825-.255.825-.57 0-.285-.015-1.23-.015-2.235-3.015.555-3.795-.735-4.035-1.41-.135-.345-.72-1.41-1.23-1.695-.42-.225-1.02-.78-.015-.795.945-.015 1.62.87 1.845 1.23 1.08 1.815 2.805 1.305 3.495.99.105-.78.42-1.305.765-1.605-2.67-.3-5.46-1.335-5.46-5.925 0-1.305.465-2.385 1.23-3.225-.12-.3-.54-1.53.12-3.18 0 0 1.005-.315 3.3 1.23.96-.27 1.98-.405 3-.405s2.04.135 3 .405c2.295-1.56 3.3-1.23 3.3-1.23.66 1.65.24 2.88.12 3.18.765.84 1.23 1.905 1.23 3.225 0 4.605-2.805 5.625-5.475 5.925.435.375.81 1.095.81 2.22 0 1.605-.015 2.895-.015 3.3 0 .315.225.69.825.57A12.02 12.02 0 0024 12c0-6.63-5.37-12-12-12z" />
                      </svg>
                    </UiButton>
                    <UiButton href="https://linkedin.com/in/judahsullivan" target="_blank" variant="primary" size="sm"
                      class="w-11! h-11! p-0!">
                      <svg class="w-5 h-5" fill="currentColor" viewBox="0 0 24 24">
                        <path
                          d="M20.447 20.452h-3.554v-5.569c0-1.328-.027-3.037-1.852-3.037-1.853 0-2.136 1.445-2.136 2.939v5.667H9.351V9h3.414v1.561h.046c.477-.9 1.637-1.85 3.37-1.85 3.601 0 4.267 2.37 4.267 5.455v6.286zM5.337 7.433c-1.144 0-2.063-.926-2.063-2.065 0-1.138.92-2.063 2.063-2.063 1.14 0 2.064.925 2.064 2.063 0 1.139-.925 2.065-2.064 2.065zm1.782 13.019H3.555V9h3.564v11.452zM22.225 0H1.771C.792 0 0 .774 0 1.729v20.542C0 23.227.792 24 1.771 24h20.451C23.2 24 24 23.227 24 22.271V1.729C24 .774 23.2 0 22.222 0h.003z" />
                      </svg>
                    </UiButton>
                    <UiButton href="https://twitter.com/judahsullivan" target="_blank" variant="primary" size="sm"
                      class="w-11! h-11! p-0!">
                      <svg class="w-5 h-5" fill="currentColor" viewBox="0 0 24 24">
                        <path
                          d="M18.244 2.25h3.308l-7.227 8.26 8.502 11.24H16.17l-5.214-6.817L4.99 21.75H1.68l7.73-8.835L1.254 2.25H8.08l4.713 6.231zm-1.161 17.52h1.833L7.084 4.126H5.117z" />
                      </svg>
                    </UiButton>
                  </div>
                </div>

                <div>
                  <p class="text-sm text-muted-foreground mb-2">Location</p>
                  <p class="text-lg font-medium">Available for Remote Work</p>
                  <p class="text-muted-foreground text-sm mt-1">Based in the United States</p>
                </div>

                <div class="pt-4 border-t border-border/50">
                  <div class="flex items-center gap-3">
                    <span class="w-2.5 h-2.5 rounded-full bg-success animate-pulse" />
                    <span class="text-base font-medium">Open to new opportunities</span>
                  </div>
                </div>
              </div>
            </div>

            <!-- FAQ -->
            <div class="bg-card/50 backdrop-blur-xl rounded-2xl p-6 sm:p-8 border border-border/50 shadow-sm">
              <UiTitle as="h3" size="md" weight="semibold" class="mb-6">Quick Questions?</UiTitle>
              <div class="space-y-5">
                <div class="flex justify-between items-center py-2 border-b border-border/30">
                  <span class="text-sm text-muted-foreground">Response time</span>
                  <span class="text-sm font-medium">24-48 hours</span>
                </div>
                <div class="flex justify-between items-center py-2 border-b border-border/30">
                  <span class="text-sm text-muted-foreground">Project types</span>
                  <span class="text-sm font-medium">Web apps, APIs, consulting</span>
                </div>
                <div class="flex justify-between items-center py-2">
                  <span class="text-sm text-muted-foreground">Tech stack</span>
                  <span class="text-sm font-medium">Vue/Nuxt, .NET, PostgreSQL</span>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>
  </div>
</template>

<script setup lang="ts">
useHead({
  title: 'Contact | Judah Sullivan',
})

const config = useRuntimeConfig()

const form = reactive({
  name: '',
  email: '',
  subject: '',
  message: '',
})

const isSubmitting = ref(false)
const successMessage = ref('')
const errorMessage = ref('')

const handleSubmit = async () => {
  isSubmitting.value = true
  errorMessage.value = ''
  successMessage.value = ''

  try {
    await $fetch(`${config.public.apiBase}/api/contact`, {
      method: 'POST',
      body: form,
    })

    successMessage.value = "Message sent successfully! I'll get back to you soon."

    // Reset form
    form.name = ''
    form.email = ''
    form.subject = ''
    form.message = ''
  } catch (error: any) {
    errorMessage.value = error?.data?.message || 'Failed to send message. Please try again.'
  } finally {
    isSubmitting.value = false
  }
}
</script>
