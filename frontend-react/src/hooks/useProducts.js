//Nesse arquivo fazemos as chamadas a API (CRUD) ONDE o app consome a API REST para fazer o CRUD de “Produto”

import { useCallback, useState } from 'react'

export function useProducts() {
  const [loading, setLoading] = useState(false)
  const [items, setItems] = useState([])

  const fetchAll = useCallback(async () => {
    setLoading(true)
    try {
      const res = await fetch('/api/products')
      setItems(await res.json())
    } finally {
      setLoading(false)
    }
  }, [])

  const createProduct = useCallback(async (dto) => {
    const res = await fetch('/api/products', {
      method: 'POST',
      headers: {'Content-Type':'application/json'},
      body: JSON.stringify(dto)
    })
    if (!res.ok) {
      try {
        const err = await res.json()
        alert(formatValidation(err) || 'Erro ao criar.')
      } catch { alert('Erro ao criar.') }
      return false
    }
    return true
  }, [])

  const updateProduct = useCallback(async (id, dto) => {
    const res = await fetch(`/api/products/${id}`, {
      method: 'PUT',
      headers: {'Content-Type':'application/json'},
      body: JSON.stringify(dto)
    })
    if (!res.ok) {
      try {
        const err = await res.json()
        alert(formatValidation(err) || 'Erro ao atualizar.')
      } catch { alert('Erro ao atualizar.') }
      return false
    }
    return true
  }, [])

  const deleteProduct = useCallback(async (id) => {
    await fetch(`/api/products/${id}`, { method: 'DELETE' })
  }, [])

  return { loading, items, fetchAll, createProduct, updateProduct, deleteProduct }
}

function formatValidation(err) {
  if (!err) return ''
  if (Array.isArray(err)) return err.map(x => x.errorMessage).join('\n')
  if (err.errors && typeof err.errors === 'object') {
    return Object.values(err.errors).flat().join('\n')
  }
  return err.title || ''
}
