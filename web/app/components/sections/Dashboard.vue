<template>
  <section class="py-12 sm:py-16 lg:py-20">
    <div class="px-4 sm:px-6 lg:px-8 max-w-7xl mx-auto">
      <!-- Header -->
      <div class="mb-8 sm:mb-12">
        <div class="flex items-center justify-between mb-3">
          <div>
            <UiTitle as="h2" size="4xl" weight="bold" align="left" class="text-3xl sm:text-4xl">
              Dashboard
            </UiTitle>
            <p class="text-base sm:text-lg text-muted-foreground max-w-2xl leading-relaxed mt-2">
              Overview of projects, metrics, and activity
            </p>
          </div>
          <div class="flex items-center gap-3">
            <NuxtLink
              to="/"
              class="inline-flex items-center gap-2 px-4 py-2 text-sm font-medium text-foreground hover:text-primary transition-colors border border-border rounded-lg hover:border-primary/50"
            >
              <Icon name="heroicons:home" class="w-4 h-4" />
              Back to Site
            </NuxtLink>
            <button
              @click="handleLogout"
              class="inline-flex items-center gap-2 px-4 py-2 text-sm font-medium text-destructive hover:text-destructive/80 transition-colors border border-destructive/20 rounded-lg hover:border-destructive/40 hover:bg-destructive/10"
            >
              <Icon name="heroicons:arrow-right-on-rectangle" class="w-4 h-4" />
              Logout
            </button>
          </div>
        </div>
      </div>

      <!-- Top Row: Main Box + 2 Stacked Boxes -->
      <div class="grid grid-cols-1 md:grid-cols-3 gap-4 sm:gap-6 mb-4 sm:mb-6">
        <!-- MAIN BOX - Most Important: Project Status (2 cols) -->
        <div class="md:col-span-2 bg-card/80 backdrop-blur-xl rounded-2xl p-6 sm:p-8 lg:p-10 border-2 border-border shadow-lg hover:border-primary/50 transition-all duration-300">
          <div class="flex items-center justify-between mb-6">
            <h3 class="text-xl sm:text-2xl font-bold text-foreground">Project Status</h3>
            <div class="w-12 h-12 rounded-xl bg-primary/20 flex items-center justify-center">
              <Icon name="heroicons:chart-bar" class="w-6 h-6 text-primary" />
            </div>
          </div>
          <div class="space-y-6">
            <div>
              <div class="flex items-center justify-between mb-3">
                <div>
                  <p class="text-sm font-semibold text-foreground mb-1">E-commerce Platform</p>
                  <p class="text-xs text-muted-foreground">Due in 2 weeks</p>
                </div>
                <span class="text-lg font-bold text-primary">85%</span>
              </div>
              <div class="w-full h-3 bg-muted rounded-full overflow-hidden">
                <div class="h-full bg-primary rounded-full transition-all duration-500" style="width: 85%"></div>
              </div>
            </div>
            <div>
              <div class="flex items-center justify-between mb-3">
                <div>
                  <p class="text-sm font-semibold text-foreground mb-1">Mobile App Redesign</p>
                  <p class="text-xs text-muted-foreground">Due in 1 month</p>
                </div>
                <span class="text-lg font-bold text-blue-500">60%</span>
              </div>
              <div class="w-full h-3 bg-muted rounded-full overflow-hidden">
                <div class="h-full bg-blue-500 rounded-full transition-all duration-500" style="width: 60%"></div>
              </div>
            </div>
            <div>
              <div class="flex items-center justify-between mb-3">
                <div>
                  <p class="text-sm font-semibold text-foreground mb-1">Brand Identity</p>
                  <p class="text-xs text-muted-foreground">Due in 3 weeks</p>
                </div>
                <span class="text-lg font-bold text-success">40%</span>
              </div>
              <div class="w-full h-3 bg-muted rounded-full overflow-hidden">
                <div class="h-full bg-success rounded-full transition-all duration-500" style="width: 40%"></div>
              </div>
            </div>
            <div>
              <div class="flex items-center justify-between mb-3">
                <div>
                  <p class="text-sm font-semibold text-foreground mb-1">API Development</p>
                  <p class="text-xs text-muted-foreground">Due in 5 days</p>
                </div>
                <span class="text-lg font-bold text-purple-500">92%</span>
              </div>
              <div class="w-full h-3 bg-muted rounded-full overflow-hidden">
                <div class="h-full bg-purple-500 rounded-full transition-all duration-500" style="width: 92%"></div>
              </div>
            </div>
          </div>
        </div>

        <!-- Right Column: 2 Stacked Boxes -->
        <div class="flex flex-col gap-4 sm:gap-6">
          <!-- Actions Box with Mini Bento Boxes -->
          <div class="bg-card/80 backdrop-blur-xl rounded-2xl p-4 sm:p-5 border-2 border-border shadow-lg hover:border-primary/50 transition-all duration-300 flex-1">
            <h3 class="text-sm font-semibold text-foreground mb-4">Quick Actions</h3>
            <div class="grid grid-cols-2 gap-3">
              <!-- Create Playground -->
              <div class="rounded-xl bg-linear-to-br from-primary/20 to-primary/5 border border-border/30 p-3 flex flex-col items-center justify-center gap-2 group hover:border-primary/50 transition-colors cursor-pointer min-h-[80px]">
                <div class="w-9 h-9 rounded-lg bg-primary/20 flex items-center justify-center group-hover:bg-primary/30 transition-colors">
                  <Icon name="heroicons:sparkles" class="w-5 h-5 text-primary" />
                </div>
                <p class="text-xs font-medium text-foreground text-center leading-tight">Create Playground</p>
              </div>

              <!-- Code -->
              <div class="rounded-xl bg-linear-to-br from-blue-500/20 to-blue-500/5 border border-border/30 p-3 flex flex-col items-center justify-center gap-2 group hover:border-blue-500/50 transition-colors cursor-pointer min-h-[80px]">
                <div class="w-9 h-9 rounded-lg bg-blue-500/20 flex items-center justify-center group-hover:bg-blue-500/30 transition-colors">
                  <Icon name="heroicons:code-bracket" class="w-5 h-5 text-blue-500" />
                </div>
                <p class="text-xs font-medium text-foreground text-center leading-tight">Code</p>
              </div>

              <!-- Add Blog -->
              <div class="rounded-xl bg-linear-to-br from-success/20 to-success/5 border border-border/30 p-3 flex flex-col items-center justify-center gap-2 group hover:border-success/50 transition-colors cursor-pointer min-h-[80px]">
                <div class="w-9 h-9 rounded-lg bg-success/20 flex items-center justify-center group-hover:bg-success/30 transition-colors">
                  <Icon name="heroicons:pencil-square" class="w-5 h-5 text-success" />
                </div>
                <p class="text-xs font-medium text-foreground text-center leading-tight">Add Blog</p>
              </div>

              <!-- Add Project -->
              <div class="rounded-xl bg-linear-to-br from-purple-500/20 to-purple-500/5 border border-border/30 p-3 flex flex-col items-center justify-center gap-2 group hover:border-purple-500/50 transition-colors cursor-pointer min-h-[80px]">
                <div class="w-9 h-9 rounded-lg bg-purple-500/20 flex items-center justify-center group-hover:bg-purple-500/30 transition-colors">
                  <Icon name="heroicons:plus-circle" class="w-5 h-5 text-purple-500" />
                </div>
                <p class="text-xs font-medium text-foreground text-center leading-tight">Add Project</p>
              </div>

              <!-- Reply to Messages -->
              <div class="col-span-2 rounded-xl bg-linear-to-br from-amber-500/20 to-amber-500/5 border border-border/30 p-3 flex items-center justify-center gap-3 group hover:border-amber-500/50 transition-colors cursor-pointer min-h-[60px]">
                <div class="w-9 h-9 rounded-lg bg-amber-500/20 flex items-center justify-center group-hover:bg-amber-500/30 transition-colors shrink-0">
                  <Icon name="heroicons:chat-bubble-left-right" class="w-5 h-5 text-amber-500" />
                </div>
                <p class="text-xs font-medium text-foreground leading-tight">Reply to Messages</p>
              </div>
            </div>
          </div>

          <!-- Analytics Box -->
          <div class="bg-card/80 backdrop-blur-xl rounded-2xl p-4 sm:p-5 border-2 border-border shadow-lg hover:border-primary/50 transition-all duration-300 flex-1">
            <h3 class="text-sm font-semibold text-foreground mb-4">Analytics</h3>
            <div class="space-y-4">
              <!-- Visits -->
              <div class="flex items-center justify-between">
                <div class="flex items-center gap-2">
                  <div class="w-2 h-2 rounded-full bg-primary"></div>
                  <span class="text-xs text-muted-foreground">Visits</span>
                </div>
                <div class="text-right">
                  <p class="text-sm font-semibold text-foreground">2.4K</p>
                  <p class="text-xs text-success">+12.5%</p>
                </div>
              </div>

              <!-- Clicks -->
              <div class="flex items-center justify-between">
                <div class="flex items-center gap-2">
                  <div class="w-2 h-2 rounded-full bg-blue-500"></div>
                  <span class="text-xs text-muted-foreground">Clicks</span>
                </div>
                <div class="text-right">
                  <p class="text-sm font-semibold text-foreground">1.8K</p>
                  <p class="text-xs text-success">+8.3%</p>
                </div>
              </div>

              <!-- Page Views -->
              <div class="flex items-center justify-between">
                <div class="flex items-center gap-2">
                  <div class="w-2 h-2 rounded-full bg-success"></div>
                  <span class="text-xs text-muted-foreground">Page Views</span>
                </div>
                <div class="text-right">
                  <p class="text-sm font-semibold text-foreground">5.2K</p>
                  <p class="text-xs text-success">+15.2%</p>
                </div>
              </div>

              <!-- Bounce Rate -->
              <div class="flex items-center justify-between">
                <div class="flex items-center gap-2">
                  <div class="w-2 h-2 rounded-full bg-amber-500"></div>
                  <span class="text-xs text-muted-foreground">Bounce Rate</span>
                </div>
                <div class="text-right">
                  <p class="text-sm font-semibold text-foreground">32%</p>
                  <p class="text-xs text-success">-4.1%</p>
                </div>
              </div>

              <!-- Avg. Session -->
              <div class="flex items-center justify-between">
                <div class="flex items-center gap-2">
                  <div class="w-2 h-2 rounded-full bg-purple-500"></div>
                  <span class="text-xs text-muted-foreground">Avg. Session</span>
                </div>
                <div class="text-right">
                  <p class="text-sm font-semibold text-foreground">3m 24s</p>
                  <p class="text-xs text-success">+0.8m</p>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>

      <!-- Bottom: Container Box with 4 Mini Boxes Inside (like FeaturedWork) -->
      <div class="bg-card/50 backdrop-blur-xl rounded-2xl p-6 sm:p-8 border border-border/50 shadow-lg">
        <h3 class="text-lg font-semibold text-foreground mb-6">Quick Info & Actions</h3>
        <div class="grid grid-cols-2 md:grid-cols-4 gap-4 lg:gap-6">
          <!-- Mini Box 1: Projects -->
          <div class="rounded-xl bg-linear-to-br from-primary/20 to-primary/5 border border-border/30 p-4 sm:p-5 flex flex-col items-center justify-center gap-2 group hover:border-primary/50 transition-colors">
            <div class="w-10 h-10 rounded-lg bg-primary/20 flex items-center justify-center group-hover:bg-primary/30 transition-colors">
              <Icon name="heroicons:folder" class="w-5 h-5 text-primary" />
            </div>
            <p class="text-sm font-medium text-foreground">Projects</p>
            <p class="text-xs text-muted-foreground">24 total</p>
          </div>

          <!-- Mini Box 2: Tasks -->
          <div class="rounded-xl bg-linear-to-br from-blue-500/20 to-blue-500/5 border border-border/30 p-4 sm:p-5 flex flex-col items-center justify-center gap-2 group hover:border-blue-500/50 transition-colors">
            <div class="w-10 h-10 rounded-lg bg-blue-500/20 flex items-center justify-center group-hover:bg-blue-500/30 transition-colors">
              <Icon name="heroicons:clipboard-document-check" class="w-5 h-5 text-blue-500" />
            </div>
            <p class="text-sm font-medium text-foreground">Tasks</p>
            <p class="text-xs text-muted-foreground">8 pending</p>
          </div>

          <!-- Mini Box 3: Actions -->
          <div class="rounded-xl bg-linear-to-br from-success/20 to-success/5 border border-border/30 p-4 sm:p-5 flex flex-col items-center justify-center gap-2 group hover:border-success/50 transition-colors">
            <div class="w-10 h-10 rounded-lg bg-success/20 flex items-center justify-center group-hover:bg-success/30 transition-colors">
              <Icon name="heroicons:bolt" class="w-5 h-5 text-success" />
            </div>
            <p class="text-sm font-medium text-foreground">Actions</p>
            <p class="text-xs text-muted-foreground">Quick links</p>
          </div>

          <!-- Mini Box 4: Activity -->
          <div class="rounded-xl bg-linear-to-br from-purple-500/20 to-purple-500/5 border border-border/30 p-4 sm:p-5 flex flex-col items-center justify-center gap-2 group hover:border-purple-500/50 transition-colors">
            <div class="w-10 h-10 rounded-lg bg-purple-500/20 flex items-center justify-center group-hover:bg-purple-500/30 transition-colors">
              <Icon name="heroicons:clock" class="w-5 h-5 text-purple-500" />
            </div>
            <p class="text-sm font-medium text-foreground">Activity</p>
            <p class="text-xs text-muted-foreground">Recent updates</p>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
const { logout } = useAuth()
const router = useRouter()

const handleLogout = () => {
  logout()
  // The logout function will handle redirecting to /auth
}
// Dashboard component with bento box layout
</script>

