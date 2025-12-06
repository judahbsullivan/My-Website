export const usePageTransition = () => {
  const router = useRouter()
  const route = useRoute()
  
  const isTransitioning = useState('pageTransition.isTransitioning', () => false)
  
  // Use inject to get the navigate function from the provider
  const navigateFromProvider = inject<((href: string) => void) | null>('pageTransition.navigate', null)
  
  const navigate = (href: string) => {
    if (!href || href === route.path || isTransitioning.value) {
      return
    }
    
    if (navigateFromProvider) {
      navigateFromProvider(href)
    } else {
      router.push(href)
    }
  }
  
  return {
    navigate,
    isTransitioning: readonly(isTransitioning),
  }
}

// Helper function to get page title from pathname
export function getPageTitleFromPath(pathname: string): string {
  if (pathname === '/') {
    return 'HOME'
  }
  
  // Remove leading slash and split by '/'
  const parts = pathname.replace(/^\//, '').split('/').filter(Boolean)
  
  if (parts.length === 0) {
    return 'HOME'
  }
  
  // Get the last part and convert to title
  const lastPart = parts[parts.length - 1] ?? ''
  return lastPart
    .split('-')
    .map(word => word.charAt(0).toUpperCase() + word.slice(1).toLowerCase())
    .join(' ')
}

