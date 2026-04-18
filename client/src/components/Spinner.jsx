export default function Spinner({ size = 'md', className = '' }) {
  const sizeMap = { sm: 'h-4 w-4 border-2', md: 'h-6 w-6 border-2', lg: 'h-10 w-10 border-[3px]' }
  return (
    <div
      className={`inline-block rounded-full border-gray-600 border-t-green-400 animate-spin ${sizeMap[size] ?? sizeMap.md} ${className}`}
    />
  )
}

export function PageSpinner() {
  return (
    <div className="flex flex-col items-center justify-center py-20 gap-4">
      <Spinner size="lg" />
      <p className="text-gray-500 text-sm">Cargando...</p>
    </div>
  )
}

export function SkeletonCard() {
  return (
    <div className="bg-gray-900 border border-gray-800 rounded-xl p-4 animate-pulse">
      <div className="flex items-start justify-between mb-3">
        <div className="h-4 bg-gray-700 rounded w-2/3" />
        <div className="h-4 bg-gray-700 rounded-full w-14" />
      </div>
      <div className="h-3 bg-gray-800 rounded w-1/2 mb-4" />
      <div className="flex items-center justify-between">
        <div className="h-3 bg-gray-800 rounded w-20" />
        <div className="h-3 bg-gray-800 rounded w-16" />
      </div>
    </div>
  )
}
