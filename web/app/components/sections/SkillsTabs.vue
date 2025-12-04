<template>
  <UiBentoBox
    container
    backdrop
    border
    shadow
    padding="lg"
    rounded="2xl"
    className="mb-6 bg-accent"
  >
    <div class="flex items-center justify-between mb-6">
      <h3 class="text-xl font-bold text-foreground">Skills & Technologies</h3>
      <Icon name="heroicons:code-bracket" class="w-6 h-6 text-primary" />
    </div>
    
    <!-- Tabs -->
    <div class="mb-6">
      <div class="flex flex-wrap gap-2 border-b border-border/50 pb-4">
        <UiButton
          v-for="category in skillCategories"
          :key="category.id"
          @click="selectedCategory = category.id"
          :class="[
            'px-4 py-2 rounded-lg text-sm font-medium transition-all duration-200 flex items-center gap-2',
            selectedCategory === category.id
              ? 'bg-primary text-white'
              : 'bg-muted text-muted-foreground hover:bg-muted/80 hover:text-foreground'
          ]"
        >
          <Icon :name="category.icon" class="w-4 h-4" />
          {{ category.name }}
        </UiButton>
      </div>
    </div>

    <!-- Skills Grid -->
    <div class="grid grid-cols-2 md:grid-cols-4 gap-4">
      <UiBentoBox
        v-for="skill in currentSkills"
        :key="skill.name"
        :variant="skill.variant as 'primary' | 'secondary' | 'success' | 'warning' | 'danger' | 'info' | 'default'"
        size="md"
        padding="md"
        :className="`flex flex-col items-center justify-center gap-2 ${getBrandColor(skill.name)}`"
      >
        <div class="w-10 h-10 flex items-center justify-center">
          <Icon :name="skill.icon" class="w-8 h-8" />
        </div>
        <p class="text-xs font-medium text-foreground text-center">{{ skill.name }}</p>
      </UiBentoBox>
    </div>
  </UiBentoBox>
</template>

<script setup lang="ts">
interface SkillCategory {
  id: string
  name: string
  icon: string
  skills: Array<{
    name: string
    icon: string
    variant: string
  }>
}

interface Props {
  skillCategories: SkillCategory[]
}

const props = defineProps<Props>()

// Tab state for skills
const selectedCategory = ref(props.skillCategories[0]?.id || 'frameworks')

// Get current skills based on selected category
const currentSkills = computed(() => {
  const category = props.skillCategories.find(cat => cat.id === selectedCategory.value)
  return category?.skills || []
})

// Get brand-specific background color for each skill
const getBrandColor = (skillName: string): string => {
  const brandColors: Record<string, string> = {
    // Frameworks
    'Vue.js': 'bg-[#42b883]/10',
    'Nuxt': 'bg-[#00dc82]/10',
    'Next.js': 'bg-black/10 dark:bg-white/10',
    'React': 'bg-[#61dafb]/10',
    'Astro': 'bg-[#ff5d01]/10',
    'Svelte': 'bg-[#ff3e00]/10',
    
    // CSS & Styling
    'Tailwind CSS': 'bg-[#06b6d4]/10',
    'CSS3': 'bg-[#1572b6]/10',
    'MUI': 'bg-[#007fff]/10',
    'Shadcn UI': 'bg-[#000000]/10 dark:bg-white/10',
    
    // Backend
    'Node.js': 'bg-[#339933]/10',
    '.NET': 'bg-[#512bd4]/10',
    'Express.js': 'bg-[#000000]/10 dark:bg-white/10',
    'Directus': 'bg-[#6644ff]/10',
    'PostgreSQL': 'bg-[#336791]/10',
    'Prisma': 'bg-[#2d3748]/10 dark:bg-[#4a5568]/10',
    
    // Languages
    'TypeScript': 'bg-[#3178c6]/10',
    'JavaScript': 'bg-[#f7df1e]/10',
    'HTML5': 'bg-[#e34f26]/10',
    'Lua': 'bg-[#000080]/10',
    
    // Tools & DevOps
    'Docker': 'bg-[#2496ed]/10',
    'AWS': 'bg-[#ff9900]/10',
    'Git': 'bg-[#f05032]/10',
    'GitHub': 'bg-[#181717]/10 dark:bg-white/10',
    'Neovim': 'bg-[#57a143]/10',
    'Pulumi': 'bg-[#8a3391]/10'
  }
  
  return brandColors[skillName] || ''
}
</script>


