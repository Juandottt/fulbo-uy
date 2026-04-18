import { useEffect, useState, useCallback } from 'react'
import { useParams, useNavigate } from 'react-router-dom'
import {
  getMatch, getParticipants, joinMatch, getTeams,
  balanceTeams, getCostSplit, getPaymentStatus, confirmPayment,
} from '../api/matches'
import { useAuth } from '../context/AuthContext'
import { useToast } from '../components/Toast'
import { PageSpinner } from '../components/Spinner'

const statusLabel = { Open: 'Abierto', Full: 'Completo', Played: 'Jugado' }
const statusBadge = {
  Open: 'bg-green-500/20 text-green-400 border border-green-500/30',
  Full: 'bg-yellow-500/20 text-yellow-400 border border-yellow-500/30',
  Played: 'bg-gray-500/20 text-gray-400 border border-gray-500/30',
}

function initials(name) {
  if (!name) return '?'
  return name.split(' ').slice(0, 2).map((w) => w[0].toUpperCase()).join('')
}

const AVATAR_COLORS = [
  'bg-blue-700', 'bg-purple-700', 'bg-pink-700', 'bg-teal-700',
  'bg-orange-700', 'bg-indigo-700', 'bg-cyan-700', 'bg-rose-700',
]

function avatar(name) {
  let sum = 0
  for (const c of (name ?? '')) sum += c.charCodeAt(0)
  return AVATAR_COLORS[sum % AVATAR_COLORS.length]
}

export default function MatchDetailPage() {
  const { id } = useParams()
  const navigate = useNavigate()
  const { user, isAdmin } = useAuth()
  const toast = useToast()

  const [match, setMatch] = useState(null)
  const [participants, setParticipants] = useState([])
  const [loading, setLoading] = useState(true)
  const [joining, setJoining] = useState(false)
  const [teams, setTeams] = useState(null)
  const [costSplit, setCostSplit] = useState(null)
  const [paymentStatus, setPaymentStatus] = useState([])
  const [balancing, setBalancing] = useState(false)
  const [payingId, setPayingId] = useState(null)

  const load = useCallback(async () => {
    try {
      const [m, p] = await Promise.all([getMatch(id), getParticipants(id)])
      setMatch(m)
      setParticipants(p)
    } catch (e) {
      toast.error(e.message)
    } finally {
      setLoading(false)
    }
    try { setTeams(await getTeams(id)) } catch {}
    try {
      const [cs, ps] = await Promise.all([getCostSplit(id), getPaymentStatus(id)])
      setCostSplit(cs)
      setPaymentStatus(ps)
    } catch {}
  }, [id]) // eslint-disable-line react-hooks/exhaustive-deps

  useEffect(() => { load() }, [load])

  const handleJoin = async () => {
    setJoining(true)
    try {
      await joinMatch(id)
      toast.success('¡Te inscribiste al partido!')
      load()
    } catch (e) {
      toast.error(e.message)
    } finally {
      setJoining(false)
    }
  }

  const handleBalanceTeams = async () => {
    setBalancing(true)
    try {
      const result = await balanceTeams(id)
      setTeams(result)
      toast.success('Equipos balanceados correctamente.')
      load()
    } catch (e) {
      toast.error(e.message)
    } finally {
      setBalancing(false)
    }
  }

  const handleConfirmPayment = async (participantId) => {
    setPayingId(participantId)
    try {
      await confirmPayment(id, participantId)
      const [cs, ps] = await Promise.all([getCostSplit(id), getPaymentStatus(id)])
      setCostSplit(cs)
      setPaymentStatus(ps)
      toast.success('Pago confirmado.')
    } catch (e) {
      toast.error(e.message)
    } finally {
      setPayingId(null)
    }
  }

  if (loading) return <PageSpinner />
  if (!match) return <p className="text-red-400 text-center py-12">Partido no encontrado.</p>

  const isJoined = participants.some((p) => p.playerName === user?.name)
  const fillPct = Math.round((match.participantCount / match.maxPlayers) * 100)
  const progressColor = fillPct >= 100 ? 'bg-yellow-500' : fillPct >= 60 ? 'bg-green-400' : 'bg-green-600'

  return (
    <div className="space-y-6">
      {/* back */}
      <button
        onClick={() => navigate(-1)}
        className="flex items-center gap-1 text-gray-500 hover:text-green-400 text-sm transition-colors"
      >
        ← Volver
      </button>

      {/* Header */}
      <div className="bg-gray-900 rounded-xl p-5 border border-gray-800">
        <div className="flex items-start justify-between mb-1">
          <h1 className="text-xl font-bold text-white pr-3">{match.location}</h1>
          <span className={`text-xs px-2.5 py-1 rounded-full font-medium ${statusBadge[match.status] ?? 'text-gray-400'}`}>
            {statusLabel[match.status] ?? match.status}
          </span>
        </div>
        <p className="text-gray-400 text-sm mb-4">
          {new Date(match.date).toLocaleString('es-UY', { dateStyle: 'full', timeStyle: 'short' })}
        </p>

        {/* Stats row */}
        <div className="grid grid-cols-3 gap-3 text-center mb-4">
          <div className="bg-gray-800 rounded-lg p-3">
            <p className="text-green-400 font-bold text-lg">{match.participantCount}/{match.maxPlayers}</p>
            <p className="text-gray-500 text-xs">Jugadores</p>
          </div>
          <div className="bg-gray-800 rounded-lg p-3">
            <p className="text-green-400 font-bold text-lg">${match.fieldCost.toLocaleString('es-UY')}</p>
            <p className="text-gray-500 text-xs">Total</p>
          </div>
          <div className="bg-gray-800 rounded-lg p-3">
            <p className="text-green-400 font-bold text-lg">
              ${match.participantCount > 0
                ? Math.round(match.fieldCost / match.participantCount).toLocaleString('es-UY')
                : '-'}
            </p>
            <p className="text-gray-500 text-xs">Por jugador</p>
          </div>
        </div>

        {/* Fill progress */}
        <div className="mb-4">
          <div className="flex justify-between text-xs text-gray-500 mb-1">
            <span>Lugares ocupados</span>
            <span>{fillPct}%</span>
          </div>
          <div className="h-2 bg-gray-800 rounded-full overflow-hidden">
            <div
              className={`h-full rounded-full transition-all duration-700 ${progressColor}`}
              style={{ width: `${Math.min(fillPct, 100)}%` }}
            />
          </div>
        </div>

        {match.status === 'Open' && !isJoined && (
          <button
            onClick={handleJoin}
            disabled={joining}
            className="w-full bg-green-600 hover:bg-green-500 disabled:opacity-50 text-white font-semibold py-2.5 rounded-lg transition-colors flex items-center justify-center gap-2"
          >
            {joining ? (
              <span className="inline-block h-4 w-4 rounded-full border-2 border-white/30 border-t-white animate-spin" />
            ) : '⚽'}
            {joining ? 'Inscribiendo...' : 'Unirme al partido'}
          </button>
        )}
        {isJoined && (
          <div className="flex items-center justify-center gap-2 py-2 bg-green-900/20 border border-green-700/30 rounded-lg">
            <span className="text-green-400 text-sm font-medium">✓ Ya estás inscripto</span>
          </div>
        )}
      </div>

      {/* Participants */}
      <div className="bg-gray-900 rounded-xl p-5 border border-gray-800">
        <h2 className="text-white font-semibold mb-4">
          Jugadores inscriptos <span className="text-gray-500 font-normal">({participants.length})</span>
        </h2>
        {participants.length === 0 ? (
          <p className="text-gray-500 text-sm">Todavía no hay jugadores inscriptos.</p>
        ) : (
          <ul className="space-y-2">
            {participants.map((p) => (
              <li key={p.id} className="flex items-center gap-3 bg-gray-800 rounded-lg px-3 py-2">
                <div className={`w-8 h-8 rounded-full flex items-center justify-center text-xs font-bold text-white shrink-0 ${avatar(p.playerName)}`}>
                  {initials(p.playerName)}
                </div>
                <span className="text-gray-200 text-sm flex-1">{p.playerName}</span>
                <div className="flex items-center gap-2">
                  {p.teamNumber > 0 && (
                    <span className={`text-xs px-2 py-0.5 rounded-full ${
                      p.teamNumber === 1
                        ? 'bg-blue-500/20 text-blue-400 border border-blue-500/20'
                        : 'bg-orange-500/20 text-orange-400 border border-orange-500/20'
                    }`}>
                      E{p.teamNumber}
                    </span>
                  )}
                  {p.hasAid ? (
                    <span className="text-xs text-green-400 font-medium">✓ Pagó</span>
                  ) : (
                    <span className="text-xs text-gray-600">Pendiente</span>
                  )}
                </div>
              </li>
            ))}
          </ul>
        )}
      </div>

      {/* Teams */}
      {(match.status === 'Full' || match.status === 'Played') && (
        <div className="bg-gray-900 rounded-xl p-5 border border-gray-800">
          <div className="flex items-center justify-between mb-4">
            <h2 className="text-white font-semibold">Equipos</h2>
            {isAdmin && match.status === 'Full' && (
              <button
                onClick={handleBalanceTeams}
                disabled={balancing}
                className="bg-green-700 hover:bg-green-600 disabled:opacity-50 text-white text-xs font-semibold px-3 py-1.5 rounded-lg transition-colors flex items-center gap-1.5"
              >
                {balancing && (
                  <span className="inline-block h-3 w-3 rounded-full border-2 border-white/30 border-t-white animate-spin" />
                )}
                {balancing ? 'Balanceando...' : '⚖️ Balancear equipos'}
              </button>
            )}
          </div>
          {teams && (teams.team1.players.length > 0 || teams.team2.players.length > 0) ? (
            <div className="grid grid-cols-2 gap-3">
              {[teams.team1, teams.team2].map((team) => (
                <div
                  key={team.teamNumber}
                  className={`rounded-xl p-4 ${
                    team.teamNumber === 1
                      ? 'bg-blue-900/20 border border-blue-700/30'
                      : 'bg-orange-900/20 border border-orange-700/30'
                  }`}
                >
                  <div className={`flex items-center justify-between mb-3 ${team.teamNumber === 1 ? 'text-blue-400' : 'text-orange-400'}`}>
                    <p className="font-semibold text-sm">Equipo {team.teamNumber}</p>
                    <p className="text-xs opacity-80">⭐ {team.averageSkill}</p>
                  </div>
                  <ul className="space-y-1.5">
                    {team.players.map((p) => (
                      <li key={p.id} className="flex items-center gap-2">
                        <div className={`w-6 h-6 rounded-full flex items-center justify-center text-[10px] font-bold text-white shrink-0 ${avatar(p.playerName)}`}>
                          {initials(p.playerName)}
                        </div>
                        <span className="text-gray-300 text-xs">{p.playerName}</span>
                      </li>
                    ))}
                  </ul>
                </div>
              ))}
            </div>
          ) : (
            <p className="text-gray-500 text-sm">Los equipos aún no fueron balanceados.</p>
          )}
        </div>
      )}

      {/* Costs */}
      {costSplit && (
        <div className="bg-gray-900 rounded-xl p-5 border border-gray-800">
          <h2 className="text-white font-semibold mb-4">División de costos</h2>
          <div className="grid grid-cols-3 gap-3 mb-4 text-center">
            <div className="bg-gray-800 rounded-lg p-3">
              <p className="text-green-400 font-bold">${costSplit.costPerPlayer.toLocaleString('es-UY')}</p>
              <p className="text-gray-500 text-xs">Por jugador</p>
            </div>
            <div className="bg-gray-800 rounded-lg p-3">
              <p className="text-green-400 font-bold">{costSplit.paidCount}</p>
              <p className="text-gray-500 text-xs">Pagaron</p>
            </div>
            <div className="bg-gray-800 rounded-lg p-3">
              <p className="text-yellow-400 font-bold">{costSplit.pendingCount}</p>
              <p className="text-gray-500 text-xs">Pendientes</p>
            </div>
          </div>

          {/* Payment progress bar */}
          {costSplit.playerCount > 0 && (
            <div className="mb-4">
              <div className="flex justify-between text-xs text-gray-500 mb-1">
                <span>Pagos recibidos</span>
                <span>{Math.round((costSplit.paidCount / costSplit.playerCount) * 100)}%</span>
              </div>
              <div className="h-1.5 bg-gray-800 rounded-full overflow-hidden">
                <div
                  className="h-full bg-green-600 rounded-full transition-all duration-700"
                  style={{ width: `${Math.round((costSplit.paidCount / costSplit.playerCount) * 100)}%` }}
                />
              </div>
            </div>
          )}

          {paymentStatus.length > 0 && (
            <ul className="space-y-2">
              {paymentStatus.map((p) => {
                const participant = participants.find((pa) => pa.id === p.participantId)
                return (
                  <li key={p.participantId} className="flex items-center gap-3 bg-gray-800 rounded-lg px-3 py-2">
                    <div className={`w-7 h-7 rounded-full flex items-center justify-center text-[10px] font-bold text-white shrink-0 ${avatar(p.playerName)}`}>
                      {initials(p.playerName)}
                    </div>
                    <span className="text-gray-200 text-sm flex-1">{p.playerName}</span>
                    {p.hasAid ? (
                      <span className="text-green-400 text-xs font-medium">✓ Pagó</span>
                    ) : participant ? (
                      <button
                        onClick={() => handleConfirmPayment(participant.id)}
                        disabled={payingId === participant.id}
                        className="text-xs bg-green-700 hover:bg-green-600 disabled:opacity-50 text-white px-2.5 py-1 rounded-lg transition-colors flex items-center gap-1"
                      >
                        {payingId === participant.id && (
                          <span className="inline-block h-3 w-3 rounded-full border-2 border-white/30 border-t-white animate-spin" />
                        )}
                        {payingId === participant.id ? '' : 'Confirmar pago'}
                      </button>
                    ) : (
                      <span className="text-yellow-400 text-xs">Pendiente</span>
                    )}
                  </li>
                )
              })}
            </ul>
          )}
        </div>
      )}
    </div>
  )
}
