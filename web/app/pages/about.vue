<template>
  <div class="min-h-screen pt-24 pb-20">
    <div class="px-4 sm:px-6 lg:px-8">
      <!-- Header Section -->
      <SectionsAboutHeader :header-data="aboutData.header" />

      <!-- Top Row: Intro + Stats -->
      <div class="grid grid-cols-1 md:grid-cols-3 gap-4 sm:gap-6 mb-6">
        <!-- Main Intro Box (2 cols) -->
        <SectionsAboutIntro :intro-data="aboutData.intro" :github-stats="githubStats" />

        <!-- Stats Box -->
        <SectionsAboutStats :stats-data="aboutData.stats" :github-stats="githubStats" />
      </div>

      <!-- Skills & Tech Stack -->
      <SectionsAboutSkills :skill-categories="aboutData.skillCategories" :background="aboutData.skillsBackground" />

      <!-- Experience & Education -->
      <div class="grid grid-cols-1 md:grid-cols-2 gap-4 sm:gap-6 mb-6 items-start">
        <!-- Experience -->
        <SectionsAboutExperience :experience-data="aboutData.experience" />

        <!-- Education -->
        <SectionsAboutEducation :education-data="aboutData.education" />
      </div>

      <!-- Interests & Hobbies -->
      <SectionsAboutInterests :interests-data="aboutData.interests" />

      <!-- Achievements & Highlights -->
      <SectionsAboutAchievements :achievements-data="aboutData.achievements" />

      <!-- Call to Action -->
      <SectionsCallToAction :cta-data="aboutData.callToAction" exit-animation-key="aboutCTA" />
    </div>
  </div>
</template>

<script setup lang="ts">
import aboutData from '../data/about.json'
import { runAllExitAnimations } from '../composables/usePageExitAnimations'

useHead({
  title: 'About | Judah Sullivan',
})

definePageMeta({
  pageTransition: {
    name: 'about-page',
    mode: 'out-in',
    onLeave: async (_el: Element, done: () => void) => {
      if (import.meta.server) return done()
      try {
        await runAllExitAnimations()
      } catch (e) {
        console.warn('[About] Exit animation error:', e)
      }
      done()
    }
  }
})

// Fetch GitHub stats dynamically
const { data: githubStats } = await useFetch('https://api.github.com/users/judahbsullivan', {
  default: () => null,
  server: false,
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
</script>



