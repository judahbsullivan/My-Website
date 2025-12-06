export const useIntroLoader = () => {
  const isIntroLoaderComplete = useState('introLoaderComplete', () => false)
  
  const setIntroLoaderComplete = (value: boolean) => {
    isIntroLoaderComplete.value = value
  }
  
  return {
    isIntroLoaderComplete: isIntroLoaderComplete, // Remove readonly to ensure reactivity
    setIntroLoaderComplete
  }
}

