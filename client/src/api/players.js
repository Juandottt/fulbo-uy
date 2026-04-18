import apiClient from './client'

export const createProfile = (data) =>
  apiClient.post('/players', data).then((r) => r.data)

export const getMyProfile = () =>
  apiClient.get('/players/me').then((r) => r.data)

export const getProfile = (id) =>
  apiClient.get(`/players/${id}`).then((r) => r.data)

export const updateProfile = (id, data) =>
  apiClient.put(`/players/${id}`, data).then((r) => r.data)
