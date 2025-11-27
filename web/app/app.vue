<template>
  <div ref="overlayRef" class="fixed inset-0 w-full  origin-top min-h-screen bg-black left-0 top-0">
  </div>
  <div class="relative container flex min-h-screen items-center justify-center flex-col w-full">
    <div class="w-full mx-auto max-w-6xl space-y-12">
      <h1 ref="headingRef" class=" text-9xl font-bold uppercase">Hello World!</h1>
      <p ref="paraRef" class=" max-w-xl text-pretty">
        Lorem ipsum dolor, sit amet consectetur adipisicing elit. Dicta ipsa repudiandae animi culpa veniam, quidem
        inventore ratione soluta quo, sunt delectus officiis odit? Rerum officia quo nesciunt et eaque facilis.
      </p>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, onBeforeUnmount } from 'vue'
const { $gsap, $SplitText } = useNuxtApp()

const headingRef = ref<HTMLElement | null>(null);
const overlayRef = ref<HTMLElement | null>(null);
const paraRef = ref<HTMLElement | null>(null);
let heading: any = null
let para: any = null

onMounted(() => {
  if (!headingRef.value) return;
  if (!paraRef.value) return;
  const tl = $gsap.timeline();

  // Split text
  heading = new $SplitText(headingRef.value, {
    type: 'chars, words',
    smartWrap: true,
    mask: 'chars',
    linesClass: "line++", // adds "line" class to each line element, plus an incremented one too ("line1", "line2", "line3", etc.)
  })

  para = new $SplitText(paraRef.value, {
    type: 'lines,words ',
    smartWrap: true,
    mask: 'lines',
    linesClass: "line++", // adds "line" class to each line element, plus an incremented one too ("line1", "line2", "line3", etc.)
  })



  if (overlayRef) {
    tl.to(overlayRef.value, {
      translateY: "-100%",
      ease: 'power4.inOut',
      duration: 1.2
    })
  }

  if (headingRef.value) {
    tl.from(heading.chars, {
      y: 150,
      stagger: 0.03,
      duration: 1,
      ease: 'power3.out',
    })

  }
  if (paraRef.value) {
    tl.from(para.lines, {
      autoAlpha: 0,
      y: -50,
      duration: 1,
      ease: 'power3.out',
      delay: 0.2,
    })

  }
})

onBeforeUnmount(() => {

})
</script>
