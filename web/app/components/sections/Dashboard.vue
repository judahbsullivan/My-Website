<template>
  <section class="py-12 sm:py-16 lg:py-20">
    <div class="px-4 sm:px-6 lg:px-8 max-w-7xl mx-auto">
      <!-- Header -->
      <div class="mb-8 sm:mb-12">
        <div class="flex items-center justify-between mb-3">
          <div>
            <UiTitle as="h2" size="4xl" weight="bold" align="left">
              Dashboard
            </UiTitle>
            <p class="text-base sm:text-lg text-muted-foreground max-w-2xl leading-relaxed mt-2">
              Overview of projects, metrics, and activity
            </p>
          </div>
          <div class="flex items-center gap-2">
            <UiButton
              variant="outline"
              size="sm"
            >
              <Icon name="heroicons:home" class="w-4 h-4" />
              Back to Site
            </UiButton>
            <UiButton
              variant="outline"
              size="sm"
            >
              <Icon name="heroicons:arrow-right-on-rectangle" class="w-4 h-4" />
              Logout
            </UiButton>
          </div>
         
        </div>
      </div>

      <!-- Top Row: Main Box + 2 Stacked Boxes -->
      <div class="grid grid-cols-1 md:grid-cols-3 gap-4 sm:gap-6 mb-4 sm:mb-6">
        <!-- MAIN BOX - Most Important: Project Status (2 cols) -->
        <UiBentoBox
          container
          backdrop
          border
          shadow
          padding="lg"
          rounded="2xl"
          :className="`md:col-span-2 ${dashboardData.sections.projectStatus.background}`"
        >
          <div class="flex items-center justify-between mb-6">
            <h3 class="text-xl sm:text-2xl font-bold text-foreground">Project Status</h3>
            <div class="w-12 h-12 rounded-xl bg-primary/20 flex items-center justify-center">
              <Icon name="heroicons:chart-bar" class="w-6 h-6 text-primary" />
            </div>
          </div>
          <div class="space-y-6">
            <div v-for="project in dashboardData.projectStatus" :key="project.title">
              <div class="flex items-center justify-between mb-3">
                <div>
                  <p class="text-sm font-semibold text-foreground mb-1">{{ project.title }}</p>
                  <p class="text-xs text-muted-foreground">{{ project.due }}</p>
                </div>
                <span :class="['text-lg font-bold', getTextColor(project.color)]">{{ project.progress }}%</span>
              </div>
              <div class="w-full h-3 bg-muted rounded-full overflow-hidden">
                <div :class="['h-full rounded-full transition-all duration-500', getProgressColor(project.color)]" :style="`width: ${project.progress}%`"></div>
              </div>
            </div>
          </div>
        </UiBentoBox>

        <!-- Right Column: 2 Stacked Boxes -->
        <div class="flex flex-col gap-4 sm:gap-6 max-w-full">
          <!-- Actions Box with Mini Bento Boxes -->
          <UiBentoBox
            container
            backdrop
            border
            shadow
            padding="lg"
            rounded="2xl"
            :className="`flex-1 ${dashboardData.sections.quickActions.background}`"
          >
            <h3 class="text-sm font-semibold text-foreground mb-4">Quick Actions</h3>
            <div class="grid grid-cols-2 gap-3">
              <UiBentoBox
                v-for="action in dashboardData.quickActions"
                :key="action.id"
                :to="action.to || undefined"
                :variant="action.variant as 'primary' | 'secondary' | 'success' | 'warning' | 'danger' | 'info' | 'default'"
                :size="(action.size || 'md') as 'sm' | 'md' | 'lg' | 'xl' | 'full'"
                :col-span="action.colSpan"
                :className="action.colSpan === 2 ? 'flex-row items-center justify-center' : 'flex-col items-center justify-center'"
              >
                <UiBentoBoxIcon :icon="action.icon" :variant="action.variant as 'primary' | 'secondary' | 'success' | 'warning' | 'danger' | 'info' | 'default'" />
                <p :class="[
                  'text-xs font-medium text-foreground',
                  action.colSpan === 2 ? 'leading-tight self-center' : 'text-center leading-tight'
                ]">
                  {{ action.title }}
                </p>
              </UiBentoBox>
            </div>
          </UiBentoBox>

          <!-- Analytics Box -->
          <UiBentoBox
            container
            backdrop
            border
            shadow
            padding="md"
            rounded="2xl"
            :className="`flex-1 ${dashboardData.sections.analytics.background}`"
          >
            <h3 class="text-sm font-semibold text-foreground mb-4">Analytics</h3>
            <div class="space-y-4">
              <div v-for="metric in dashboardData.analytics" :key="metric.label" class="flex items-center justify-between">
                <div class="flex items-center gap-2">
                  <div :class="['w-2 h-2 rounded-full', `bg-${metric.color === 'primary' ? 'primary' : metric.color === 'secondary' ? 'blue-500' : metric.color === 'success' ? 'success' : metric.color === 'warning' ? 'amber-500' : 'purple-500'}`]"></div>
                  <span class="text-xs text-muted-foreground">{{ metric.label }}</span>
                </div>
                <div class="text-right">
                  <p class="text-sm font-semibold text-foreground">{{ metric.value }}</p>
                  <p class="text-xs text-success">{{ metric.change }}</p>
                </div>
              </div>
            </div>
          </UiBentoBox>
        </div>
      </div>

      <!-- Bottom: Container Box with 4 Mini Boxes Inside (like FeaturedWork) -->
      <UiBentoBox
        container
        backdrop
        border
        shadow
        padding="lg"
        rounded="2xl"
        :className="dashboardData.sections.quickInfo.background"
      >
        <h3 class="text-lg font-semibold text-foreground mb-6">Quick Info & Actions</h3>
        <div class="grid grid-cols-2 md:grid-cols-4 gap-4 lg:gap-6">
          <UiBentoBox
            v-for="info in dashboardData.quickInfo"
            :key="info.title"
            :variant="info.variant as 'primary' | 'secondary' | 'success' | 'warning' | 'danger' | 'info' | 'default'"
            size="lg"
            padding="lg"
            className="flex-col gap-2"
          >
            <UiBentoBoxIcon :icon="info.icon" :variant="info.variant as 'primary' | 'secondary' | 'success' | 'warning' | 'danger' | 'info' | 'default'" size="lg" />
            <p class="text-sm font-medium text-foreground">{{ info.title }}</p>
            <p class="text-xs text-muted-foreground">{{ info.value }}</p>
          </UiBentoBox>
        </div>
      </UiBentoBox>

      <!-- Blog Actions: 4 Mini Bento Boxes -->
      <UiBentoBox
        container
        backdrop
        border
        shadow
        padding="lg"
        rounded="2xl"
        :className="`mt-6 ${dashboardData.sections.blogActions.background}`"
      >
        <h3 class="text-lg font-semibold text-foreground mb-6">Blog Actions</h3>
        <div class="grid grid-cols-2 md:grid-cols-4 gap-4 lg:gap-6">
          <UiTooltip
            v-for="action in dashboardData.blogActions"
            :key="action.id"
            :text="action.tooltip"
            placement="top"
          >
            <template #trigger>
              <UiBentoBox
                :variant="action.variant as 'primary' | 'secondary' | 'success' | 'warning' | 'danger' | 'info' | 'default'"
                size="lg"
                padding="lg"
                className="flex-col gap-3 w-full cursor-pointer"
                :to="action.to"
              >
                <UiBentoBoxIcon :icon="action.icon" :variant="action.variant as 'primary' | 'secondary' | 'success' | 'warning' | 'danger' | 'info' | 'default'" size="lg" />
                <UiButton
                  variant="ghost"
                  size="sm"
                  :to="action.to"
                  className="text-xs font-medium w-full"
                >
                  {{ action.title }}
                </UiButton>
              </UiBentoBox>
            </template>
          </UiTooltip>
        </div>
      </UiBentoBox>

      <!-- Projects & Playground Actions: 3 Column Grid -->
      <div class="grid grid-cols-1 md:grid-cols-2 gap-6 mt-6">
        <!-- Project Actions -->
        <UiBentoBox
          container
          backdrop
          border
          shadow
          padding="lg"
          rounded="2xl"
          :className="dashboardData.sections.projectActions.background"
        >
          <h3 class="text-lg font-semibold text-foreground mb-6">Project Actions</h3>
          <div class="grid grid-cols-3 gap-4">
            <UiTooltip
              v-for="action in dashboardData.projectActions"
              :key="action.id"
              :text="action.tooltip"
              placement="top"
            >
              <template #trigger>
                <UiBentoBox
                  :variant="action.variant as 'primary' | 'secondary' | 'success' | 'warning' | 'danger' | 'info' | 'default'"
                  size="lg"
                  padding="lg"
                  className="flex-col gap-3 w-full cursor-pointer"
                  :to="action.to"
                >
                  <UiBentoBoxIcon :icon="action.icon" :variant="action.variant as 'primary' | 'secondary' | 'success' | 'warning' | 'danger' | 'info' | 'default'" size="lg" />
                  <UiButton
                    variant="ghost"
                    size="sm"
                    :to="action.to"
                    className="text-xs font-medium w-full text-center"
                  >
                    {{ action.title }}
                  </UiButton>
                </UiBentoBox>
              </template>
            </UiTooltip>
          </div>
        </UiBentoBox>

        <!-- Playground Actions -->
        <UiBentoBox
          container
          backdrop
          border
          shadow
          padding="lg"
          rounded="2xl"
          :className="dashboardData.sections.playgroundActions.background"
        >
          <h3 class="text-lg font-semibold text-foreground mb-6">Playground Actions</h3>
          <div class="grid grid-cols-3 gap-4">
            <UiTooltip
              v-for="action in dashboardData.playgroundActions"
              :key="action.id"
              :text="action.tooltip"
              placement="top"
            >
              <template #trigger>
                <UiBentoBox
                  :variant="action.variant as 'primary' | 'secondary' | 'success' | 'warning' | 'danger' | 'info' | 'default'"
                  size="lg"
                  padding="lg"
                  className="flex-col gap-3 w-full cursor-pointer"
                  :to="action.to"
                >
                  <UiBentoBoxIcon :icon="action.icon" :variant="action.variant as 'primary' | 'secondary' | 'success' | 'warning' | 'danger' | 'info' | 'default'" size="lg" />
                  <UiButton
                    variant="ghost"
                    size="sm"
                    :to="action.to"
                    className="text-xs font-medium w-full text-center"
                  >
                    {{ action.title }}
                  </UiButton>
                </UiBentoBox>
              </template>
            </UiTooltip>
          </div>
        </UiBentoBox>
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
interface QuickAction {
  id: string
  title: string
  icon: string
  variant: 'primary' | 'secondary' | 'success' | 'warning' | 'danger' | 'info' | 'default'
  to: string | null
  action: string | null
  colSpan?: number
  size?: 'sm' | 'md' | 'lg' | 'xl' | 'full'
}

interface Analytics {
  label: string
  value: string
  change: string
  color: string
}

interface ProjectStatus {
  title: string
  due: string
  progress: number
  color: string
}

interface QuickInfo {
  title: string
  value: string
  icon: string
  variant: 'primary' | 'secondary' | 'success' | 'warning' | 'danger' | 'info' | 'default'
}

interface BlogAction {
  id: string
  title: string
  icon: string
  variant: 'primary' | 'secondary' | 'success' | 'warning' | 'danger' | 'info' | 'default'
  to: string
  tooltip: string
}

interface ProjectAction {
  id: string
  title: string
  icon: string
  variant: 'primary' | 'secondary' | 'success' | 'warning' | 'danger' | 'info' | 'default'
  to: string
  tooltip: string
}

interface PlaygroundAction {
  id: string
  title: string
  icon: string
  variant: 'primary' | 'secondary' | 'success' | 'warning' | 'danger' | 'info' | 'default'
  to: string
  tooltip: string
}

interface DashboardData {
  quickActions: QuickAction[]
  analytics: Analytics[]
  projectStatus: ProjectStatus[]
  quickInfo: QuickInfo[]
  blogActions: BlogAction[]
  projectActions: ProjectAction[]
  playgroundActions: PlaygroundAction[]
}

const { logout } = useAuth()
const router = useRouter()

// Load dashboard data from JSON - import directly
import dashboardJson from '../../data/dashboard.json'

// Type assertion for dashboard data with sections
const dashboardData = dashboardJson as typeof dashboardJson & {
  sections: {
    projectStatus: { background: string }
    quickActions: { background: string }
    analytics: { background: string }
    quickInfo: { background: string }
    blogActions: { background: string }
    projectActions: { background: string }
    playgroundActions: { background: string }
  }
}

// Ensure arrays exist with default empty arrays
if (!dashboardData.blogActions) {
  dashboardData.blogActions = []
}
if (!dashboardData.projectActions) {
  dashboardData.projectActions = []
}
if (!dashboardData.playgroundActions) {
  dashboardData.playgroundActions = []
}

const handleLogout = () => {
  logout()
  // The logout function will handle redirecting to /auth
}

// Get color class for progress bars
const getProgressColor = (color: string) => {
  const colors: Record<string, string> = {
    primary: 'bg-primary',
    secondary: 'bg-blue-500',
    success: 'bg-success',
    info: 'bg-purple-500'
  }
  return colors[color] || 'bg-primary'
}

// Get text color class
const getTextColor = (color: string) => {
  const colors: Record<string, string> = {
    primary: 'text-primary',
    secondary: 'text-blue-500',
    success: 'text-success',
    info: 'text-purple-500'
  }
  return colors[color] || 'text-primary'
}
</script>

