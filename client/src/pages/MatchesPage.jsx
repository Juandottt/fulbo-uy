import { useEffect, useState } from 'react'
import { Link } from 'react-router-dom'
import { getMatches } from '../api/matches'
import { useAuth } from '../context/AuthContext'
import { SkeletonCard } from '../components/Spinner'

const statusColor = {
  Open: 'bg-green-500/20 text-green-400 border border-green-500/30',
  Full: 'bg-yellow-500/20 text-yellow-400 border border-yellow-500/30',
  Played: 'bg-gray-500/20 text-gray-400 border border-gray-500/30',
}

const statusLabel = { Open: 'Abierto', Full: 'Completo', Played: 'Jugado' }

function MatchCard({ match }) {
  const fillPct = Math.round((match.participantCount / match.maxPlayers) * 100)
  const progressColor = fillPct >= 100 ? 'bg-yellow-500' : fillPct >= 60 ? 'bg-green-400' : 'bg-green-600'

  return (
    <Link
      to={`/matches/${match.id}`}
      className="bg-gray-900 border border-gray-800 hover:border-green-700 rounded-xl p-4 transition-colors group flex flex-col"
    >
      <div className="flex items-start justify-between mb-2">
        <h2 className="text-white font-semibold group-hover:text-green-400 transition-colors leading-snug pr-2">
          {match.location}
        </h2>
        <span className={`text-xs px-2 py-0.5 rounded-full shrink-0 ${statusColor[match.status] ?? ''}`}>
          {statusLabel[match.status] ?? match.status}
        </span>
      </div>
      <p className="text-gray-400 text-sm mb-3">
        {new Date(match.date).toLocaleString('es-UY', {
          dateStyle: 'medium',
          timeStyle: 'short',
        })}
      </p>

      {/* progress bar */}
      <div className="mt-auto">
        <div className="flex items-center justify-between text-xs text-gray-500 mb-1">
          <span>{match.participantCount} / {match.maxPlayers} jugadores</span>
          <span>${match.fieldCost.toLocaleString('es-UY')}</span>
        </div>
        <div className="h-1.5 bg-gray-800 rounded-full overflow-hidden">
          <div
            className={`h-full rounded-full transition-all duration-500 ${progressColor}`}
            style={{ width: `${Math.min(fillPct, 100)}%` }}
          />
        </div>
      </div>
    </Link>
  )
}

export default function MatchesPage() {
  const [matches, setMatches] = useState([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')
  const { isAdmin } = useAuth()

  useEffect(() => {
    getMatches()
      .then(setMatches)
      .catch((e) => setError(e.message))
      .finally(() => setLoading(false))
  }, [])

  return (
    <div>
      <div className="flex items-center justify-between mb-6">
        <h1 className="text-2xl font-bold text-white">Partidos</h1>
        {isAdmin && (
          <Link
            to="/matches/new"
            className="bg-green-600 hover:bg-green-500 text-white text-sm font-semibold px-4 py-2 rounded-lg transition-colors"
          >
            + Nuevo partido
          </Link>
        )}
      </div>

      {error && (
        <div className="text-red-400 text-sm bg-red-900/20 border border-red-800/40 rounded-xl px-4 py-3 mb-4">
          {error}
        </div>
      )}

      {loading ? (
        <div className="grid gap-3 sm:grid-cols-2">
          {[1, 2, 3, 4].map((i) => <SkeletonCard key={i} />)}
        </div>
      ) : matches.length === 0 ? (
        <div className="flex flex-col items-center justify-center py-20 text-center">
          <div className="text-6xl mb-4 opacity-30">⚽</div>
          <p className="text-gray-400 font-medium mb-1">No hay partidos disponibles</p>
          <p className="text-gray-600 text-sm">
            {isAdmin ? 'Creá el primero con el botón de arriba.' : 'Volvé más tarde o pedile al admin que cree uno.'}
          </p>
        </div>
      ) : (
        <div className="grid gap-3 sm:grid-cols-2">
          {matches.map((match) => (
            <MatchCard key={match.id} match={match} />
          ))}
        </div>
      )}
    </div>
  )
}
