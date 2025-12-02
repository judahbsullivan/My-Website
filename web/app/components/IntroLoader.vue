<template>
  <ClientOnly>
    <div v-if="showLoader" class="fixed inset-0 z-999999 pointer-events-none overflow-hidden">
      <!-- Black Overlay -->
      <div 
        ref="overlayRef"
        class="fixed inset-0 bg-black z-999998 origin-center scale-y-100"
      />
      
      <!-- Text Container -->
      <div class="fixed left-1/2 top-1/2 -translate-x-1/2 -translate-y-1/2 z-1000002 text-center pointer-events-none">
        <!-- Full Name -->
        <div 
          ref="nameRef"
          class="text-5xl font-bold text-white whitespace-nowrap leading-tight"
        >
          Judah Sullivan
        </div>
      </div>
    </div>
    <template #fallback>
      <div></div>
    </template>
  </ClientOnly>
</template>

<script setup lang="ts">
import { ref, onMounted, nextTick } from 'vue'
import { useNuxtApp } from '#app'
import { gsap } from 'gsap'
import { SplitText } from 'gsap/SplitText'

const showLoader = ref(false)
const overlayRef = ref<HTMLElement | null>(null)
const nameRef = ref<HTMLElement | null>(null)

onMounted(() => {
  if (import.meta.server) {
    showLoader.value = false
    return
  }
  
  // Show loader on mount (reload)
  showLoader.value = true
  
  // Wait for DOM to be ready
  nextTick(() => {
    setTimeout(() => {
      startAnimation()
    }, 200)
  })
})

function startAnimation() {
  const nuxtApp = useNuxtApp()
  const gsap = nuxtApp.$gsap as typeof import('gsap').gsap
  const SplitText = nuxtApp.$SplitText as any
  const { setIntroLoaderComplete } = useIntroLoader()
  
  if (!gsap || !SplitText) {
    showLoader.value = false
    setIntroLoaderComplete(true)
    return
  }
  
  // Wait for refs to be ready
  nextTick(() => {
    if (!overlayRef.value || !nameRef.value) {
      setTimeout(() => startAnimation(), 50)
      return
    }
    
    const overlay = overlayRef.value
    const nameEl = nameRef.value
    
    // Set overlay initial state - starts full screen
    gsap.set(overlay, {
      scaleY: 1,
      transformOrigin: 'center center',
      opacity: 1
    })
    
    // Split name with mask
    let nameSplit: any = null
    try {
      nameSplit = new SplitText(nameEl, { 
        type: 'chars',
        mask: 'chars'
      })
      if (nameSplit.chars && nameSplit.chars.length > 0) {
        gsap.set(nameSplit.chars, {
          yPercent: 150 // Hide characters initially
        })
      }
    } catch (e) {
      console.warn('SplitText failed for name', e)
    }
    
    // Animation timeline
    const tl = gsap.timeline()
    
    // Show "Judah Sullivan" with SplitText mask
    if (nameSplit && nameSplit.chars) {
      tl.to(nameSplit.chars, {
        yPercent: 0,
        duration: 0.8,
        stagger: 0.03,
        ease: 'power2.out'
      }, '+=0.3')
    }
    
    // Hold for a moment
    tl.to({}, { duration: 0.5 })
    
    // Hide name
    if (nameSplit && nameSplit.chars) {
      tl.to(nameSplit.chars, {
        yPercent: -150,
        duration: 0.4,
        stagger: 0.02,
        ease: 'power2.in'
      })
    }
    
    // Black overlay slides away from top to reveal page
    tl.to(overlay, {
      scaleY: 0,
      transformOrigin: 'top center',
      duration: 1,
      ease: 'power3.inOut'
    }, '-=0.2')
    
    // Cleanup
    tl.call(() => {
      if (nameSplit && nameSplit.revert) nameSplit.revert()
      showLoader.value = false
      setIntroLoaderComplete(true) // Signal that intro loader is complete
    })
  })
}

</script>

<style scoped>
/* SplitText mask styles */
:deep(.char) {
  display: inline-block;
  overflow: hidden;
  padding: 2px 0;
}

:deep(.line) {
  overflow: hidden;
  line-height: 1.2;
}
</style>
