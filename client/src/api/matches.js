import apiClient from './client'

export const getMatches = () =>
  apiClient.get('/matches').then((r) => r.data)

export const getMatch = (id) =>
  apiClient.get(`/matches/${id}`).then((r) => r.data)

export const createMatch = (data) =>
  apiClient.post('/matches', data).then((r) => r.data)

export const joinMatch = (id) =>
  apiClient.post(`/matches/${id}/join`).then((r) => r.data)

export const getParticipants = (id) =>
  apiClient.get(`/matches/${id}/participants`).then((r) => r.data)

export const balanceTeams = (id) =>
  apiClient.post(`/matches/${id}/balance-teams`).then((r) => r.data)

export const getTeams = (id) =>
  apiClient.get(`/matches/${id}/teams`).then((r) => r.data)

export const getCostSplit = (id) =>
  apiClient.get(`/matches/${id}/cost-split`).then((r) => r.data)

export const confirmPayment = (matchId, participantId) =>
  apiClient.post(`/matches/${matchId}/participants/${participantId}/pay`).then((r) => r.data)

export const getPaymentStatus = (id) =>
  apiClient.get(`/matches/${id}/payment-status`).then((r) => r.data)
