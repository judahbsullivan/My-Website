<template>
  <div class="min-h-screen pt-24 pb-20">
    <div class="px-4 sm:px-6 lg:px-8 ">
      <!-- Header Section -->
      <div class="mb-12 sm:mb-16">
        <UiTitle as="h1" size="5xl" weight="bold" align="left" class="mb-6">
          About Me
        </UiTitle>
        <p class="text-lg sm:text-xl text-muted-foreground max-w-3xl leading-relaxed">
          {{ aboutData.bio.tagline }}
        </p>
      </div>

      <!-- Top Row: Intro + Stats -->
      <div class="grid grid-cols-1 md:grid-cols-3 gap-4 sm:gap-6 mb-6">
        <!-- Main Intro Box (2 cols) -->
        <UiBentoBox
          container
          backdrop
          border
          shadow
          padding="lg"
          rounded="2xl"
          :className="`md:col-span-2 ${aboutData.sections.intro.background}`"
        >
          <div class="flex flex-col sm:flex-row items-start sm:items-center gap-4 mb-6">
            <div class="w-16 h-16 rounded-xl overflow-hidden border-2 border-border bg-primary/20 flex items-center justify-center shrink-0">
              <img 
                v-if="githubStats?.avatar_url" 
                :src="githubStats.avatar_url" 
                alt="Judah Sullivan"
                class="w-full h-full object-cover"
              />
              <Icon v-else name="heroicons:user" class="w-8 h-8 text-primary" />
            </div>
            <div class="flex-1 min-w-0">
              <div class="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-2 sm:gap-4">
                <div class="flex-1 min-w-0">
                  <h2 class="text-xl sm:text-2xl font-bold text-foreground">Judah Sullivan</h2>
                  <p class="text-sm sm:text-base text-muted-foreground">{{ aboutData.bio.title }}</p>
                  <div class="flex flex-wrap items-center gap-2 sm:gap-3 mt-2">
                    <span class="text-xs text-muted-foreground flex items-center gap-1">
                      <Icon name="heroicons:map-pin" class="w-4 h-4 shrink-0" />
                      <span class="truncate">{{ aboutData.bio.location }}</span>
                    </span>
                    <span v-if="githubStats" class="text-xs text-muted-foreground flex items-center gap-1">
                      <Icon name="heroicons:code-bracket-square" class="w-4 h-4 shrink-0" />
                      <span>{{ githubStats.public_repos }} repos</span>
                    </span>
                  </div>
                </div>
                <div v-if="aboutData.bio.status" class="px-3 py-1 text-xs font-medium rounded-full bg-success/20 text-success shrink-0">
                  {{ aboutData.bio.status }}
                </div>
              </div>
            </div>
          </div>
          <p class="text-sm sm:text-base text-foreground leading-relaxed mb-4">
            {{ aboutData.bio.description }}
          </p>
          <div class="flex flex-wrap items-center gap-3 sm:gap-4 pt-4 border-t border-border/50">
            <a 
              :href="aboutData.bio.github" 
              target="_blank" 
              rel="noopener noreferrer"
              class="text-xs sm:text-sm text-primary hover:underline flex items-center gap-2"
            >
              <Icon name="heroicons:code-bracket-square" class="w-4 h-4 shrink-0" />
              GitHub
            </a>
            <a 
              v-if="aboutData.bio.website"
              :href="aboutData.bio.website" 
              target="_blank" 
              rel="noopener noreferrer"
              class="text-xs sm:text-sm text-primary hover:underline flex items-center gap-2"
            >
              <Icon name="heroicons:globe-alt" class="w-4 h-4 shrink-0" />
              Website
            </a>
          </div>
        </UiBentoBox>

        <!-- Stats Box -->
        <UiBentoBox
          container
          backdrop
          border
          shadow
          padding="lg"
          rounded="2xl"
          :className="aboutData.sections.stats.background"
        >
          <h3 class="text-lg font-semibold text-foreground mb-6">Quick Stats</h3>
          <div class="space-y-6">
            <div v-for="stat in aboutData.stats" :key="stat.label" class="flex items-center justify-between">
              <div class="flex items-center gap-3">
                <div :class="[
                  'w-10 h-10 rounded-lg flex items-center justify-center',
                  stat.color === 'primary' ? 'bg-primary/20' : 
                  stat.color === 'secondary' ? 'bg-blue-500/20' : 
                  stat.color === 'success' ? 'bg-success/20' : 
                  'bg-amber-500/20'
                ]">
                  <Icon :name="stat.icon" :class="[
                    'w-5 h-5',
                    stat.color === 'primary' ? 'text-primary' : 
                    stat.color === 'secondary' ? 'text-blue-500' : 
                    stat.color === 'success' ? 'text-success' : 
                    'text-amber-500'
                  ]" />
                </div>
                <span class="text-sm text-muted-foreground">{{ stat.label }}</span>
              </div>
              <p class="text-lg font-bold text-foreground">
                <template v-if="stat.label === 'GitHub Repos' && githubStats">
                  {{ githubStats.public_repos }}+
                </template>
                <template v-else>
                  {{ stat.value }}
                </template>
              </p>
            </div>
            <div v-if="githubStats" class="pt-4 border-t border-border/50">
              <div class="flex items-center justify-between text-xs text-muted-foreground">
                <span>Followers</span>
                <span class="font-semibold text-foreground">{{ githubStats.followers }}</span>
              </div>
              <div class="flex items-center justify-between text-xs text-muted-foreground mt-2">
                <span>Following</span>
                <span class="font-semibold text-foreground">{{ githubStats.following }}</span>
              </div>
            </div>
          </div>
        </UiBentoBox>
      </div>

      <!-- Skills & Tech Stack -->
      <SectionsSkillsTabs :skill-categories="aboutData.skillCategories" />

      <!-- Experience & Education -->
      <div class="grid grid-cols-1 md:grid-cols-2 gap-4 sm:gap-6 mb-6 items-start">
        <!-- Experience -->
        <UiBentoBox
          container
          backdrop
          border
          shadow
          padding="lg"
          rounded="2xl"
          :className="aboutData.sections.experience.background"
        >
          <div class="flex items-center gap-3 mb-6">
            <div class="w-12 h-12 rounded-xl bg-primary/20 flex items-center justify-center">
              <Icon name="heroicons:briefcase" class="w-6 h-6 text-primary" />
            </div>
            <h3 class="text-xl font-bold text-foreground">Experience</h3>
          </div>
          <div class="space-y-6">
            <div v-for="exp in aboutData.experience" :key="exp.title" class="border-l-2 border-primary/30 pl-4">
              <div class="flex items-start justify-between mb-2">
                <div class="flex-1">
                  <h4 class="text-base font-semibold text-foreground">{{ exp.title }}</h4>
                  <p class="text-sm text-muted-foreground">{{ exp.company }}</p>
                  <p class="text-xs text-muted-foreground mt-1">{{ exp.location }} · {{ exp.type }}</p>
                </div>
                <span class="text-xs text-muted-foreground whitespace-nowrap ml-4">{{ exp.period }}</span>
              </div>
              <p class="text-sm text-foreground mb-3">{{ exp.description }}</p>
              <ul class="space-y-1.5">
                <li v-for="(responsibility, index) in exp.responsibilities.slice(0, 3)" :key="index" class="text-xs text-muted-foreground flex items-start gap-2">
                  <span class="text-primary mt-1.5">•</span>
                  <span>{{ responsibility }}</span>
                </li>
              </ul>
            </div>
          </div>
        </UiBentoBox>

        <!-- Education -->
        <UiBentoBox
          container
          backdrop
          border
          shadow
          padding="lg"
          rounded="2xl"
          :className="`self-start ${aboutData.sections.education.background}`"
        >
          <div class="flex items-center gap-3 mb-6">
            <div class="w-12 h-12 rounded-xl bg-secondary/20 flex items-center justify-center">
              <Icon name="heroicons:academic-cap" class="w-6 h-6 text-blue-500" />
            </div>
            <h3 class="text-xl font-bold text-foreground">Education</h3>
          </div>
          <div class="space-y-6">
            <div v-for="edu in aboutData.education" :key="edu.degree" class="border-l-2 border-blue-500/30 pl-4">
              <div class="flex items-start justify-between mb-2">
                <div>
                  <h4 class="text-base font-semibold text-foreground">{{ edu.degree }}</h4>
                  <p class="text-sm text-muted-foreground">{{ edu.institution }}</p>
                </div>
                <span class="text-xs text-muted-foreground whitespace-nowrap">{{ edu.year }}</span>
              </div>
              <p class="text-sm text-foreground">{{ edu.description }}</p>
            </div>
          </div>
        </UiBentoBox>
      </div>

      <!-- Interests & Hobbies -->
      <UiBentoBox
        container
        backdrop
        border
        shadow
        padding="lg"
        rounded="2xl"
        :className="`mb-6 ${aboutData.sections.interests.background}`"
      >
        <div class="flex items-center justify-between mb-6">
          <h3 class="text-xl font-bold text-foreground">Interests & Hobbies</h3>
          <Icon name="heroicons:sparkles" class="w-6 h-6 text-primary" />
        </div>
        <div class="grid grid-cols-2 md:grid-cols-4 gap-4">
          <UiBentoBox
            v-for="interest in aboutData.interests"
            :key="interest.name"
            :variant="interest.variant as 'primary' | 'secondary' | 'success' | 'warning' | 'danger' | 'info' | 'default'"
            size="md"
            padding="md"
            :className="`flex-col items-center justify-center gap-2 ${getInterestColor(interest.name)}`"
          >
            <UiBentoBoxIcon 
              v-if="interest.name !== 'Coding'"
              :icon="interest.icon" 
              :variant="interest.variant as 'primary' | 'secondary' | 'success' | 'warning' | 'danger' | 'info' | 'default'" 
            />
            <div v-else class="w-8 h-8 rounded-lg flex items-center justify-center bg-purple-500/20">
              <Icon :name="interest.icon" class="w-4 h-4 text-purple-500" />
            </div>
            <p class="text-xs font-medium text-foreground text-center">{{ interest.name }}</p>
            <p v-if="interest.description" class="text-xs text-muted-foreground text-center mt-1">{{ interest.description }}</p>
          </UiBentoBox>
        </div>
      </UiBentoBox>

      <!-- Achievements & Highlights -->
      <div class="grid grid-cols-1 md:grid-cols-3 gap-4 sm:gap-6 mb-6">
        <UiBentoBox
          v-for="achievement in aboutData.achievements"
          :key="achievement.title"
          :variant="achievement.variant as 'primary' | 'secondary' | 'success' | 'warning' | 'danger' | 'info' | 'default'"
          size="lg"
          padding="lg"
          :className="`flex-col items-start gap-3 ${getAchievementColor(achievement.title)}`"
        >
          <div class="flex items-center gap-3 w-full">
            <UiBentoBoxIcon :icon="achievement.icon" :variant="achievement.variant as 'primary' | 'secondary' | 'success' | 'warning' | 'danger' | 'info' | 'default'" size="lg" />
            <h4 class="text-base font-semibold text-foreground">{{ achievement.title }}</h4>
          </div>
          <p class="text-sm text-muted-foreground">{{ achievement.description }}</p>
        </UiBentoBox>
      </div>

      <!-- Call to Action -->
      <UiBentoBox
        container
        backdrop
        border
        shadow
        padding="xl"
        rounded="2xl"
        :className="`text-center ${aboutData.sections.callToAction.background}`"
      >
        <div class="max-w-2xl mx-auto">
          <Icon name="heroicons:chat-bubble-left-right" class="w-12 h-12 text-primary mx-auto mb-4" />
          <h3 class="text-2xl font-bold text-foreground mb-4">Let's Work Together</h3>
          <p class="text-base text-muted-foreground mb-6">
            Have a project in mind? I'm always open to discussing new opportunities and interesting challenges.
          </p>
          <div class="flex flex-col sm:flex-row gap-4 justify-center">
            <UiButton to="/contact" variant="primary" size="lg">
              <Icon name="heroicons:envelope" class="w-5 h-5" />
              Get in Touch
            </UiButton>
            <UiButton to="/projects" variant="outline" size="lg">
              <Icon name="heroicons:folder" class="w-5 h-5" />
              View My Work
            </UiButton>
          </div>
        </div>
      </UiBentoBox>
    </div>
  </div>
</template>

<script setup lang="ts">
import aboutData from '../data/about.json'

useHead({
  title: 'About | Judah Sullivan',
})

// Fetch GitHub stats dynamically
const { data: githubStats } = await useFetch('https://api.github.com/users/judahbsullivan', {
  default: () => null,
  server: false, // Only fetch on client to avoid CORS issues
  transform: (data: any) => {
    if (!data) return null
    return {
      avatar_url: data.avatar_url,
      public_repos: data.public_repos,
      followers: data.followers,
      following: data.following,
      created_at: data.created_at
    }
  }
})

// Get interest-specific background color
const getInterestColor = (interestName: string): string => {
  const interestColors: Record<string, string> = {
    'Coding': 'bg-purple-500/10',
    'Guitar': 'bg-blue-500/10',
    'Basketball': 'bg-orange-500/10',
    'Family': 'bg-pink-500/10'
  }
  
  return interestColors[interestName] || ''
}

// Get achievement-specific background color
const getAchievementColor = (achievementTitle: string): string => {
  const achievementColors: Record<string, string> = {
    'Open Source': 'bg-amber-500/10',
    'Blog Writer': 'bg-green-500/10',
    'Community': 'bg-blue-500/10'
  }
  
  return achievementColors[achievementTitle] || ''
}

</script>
