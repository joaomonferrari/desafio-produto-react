import { useState } from 'react'

export function ProductForm({ onCreate }) {
  const [name, setName] = useState('')
  const [category, setCategory] = useState('')
  const [price, setPrice] = useState('')
  const [quantity, setQuantity] = useState('')
  const [createdAt, setCreatedAt] = useState('')

  const clear = () => {
    setName(''); setCategory(''); setPrice(''); setQuantity(''); setCreatedAt('')
  }

  const submit = async () => {
    const dto = {
      name: name.trim(),
      category: category.trim(),
      price: parseFloat(price || 0),
      quantityInStock: parseInt(quantity || 0),
      createdAt: createdAt ? new Date(createdAt).toISOString() : new Date().toISOString()
    }
    const ok = await onCreate(dto)
    if (ok !== false) clear()
  }

  return (
    <>
      <div className="field">
        <label className="label">Nome</label>
        <input className="input" value={name} onChange={e=>setName(e.target.value)} placeholder="Teclado Mecânico" />
      </div>
      <div className="field">
        <label className="label">Categoria</label>
        <input className="input" value={category} onChange={e=>setCategory(e.target.value)} placeholder="Eletrônicos" />
      </div>
      <div className="row">
        <div className="field" style={{flex:'1 1 180px'}}>
          <label className="label">Preço</label>
          <input className="input" type="number" step="0.01" value={price} onChange={e=>setPrice(e.target.value)} />
        </div>
        <div className="field" style={{flex:'1 1 180px'}}>
          <label className="label">Quantidade</label>
          <input className="input" type="number" value={quantity} onChange={e=>setQuantity(e.target.value)} />
        </div>
      </div>
      <div className="field">
        <label className="label">Data de inclusão</label>
        <input className="input" type="datetime-local" value={createdAt} onChange={e=>setCreatedAt(e.target.value)} />
      </div>
      <div className="actions">
        <button className="btn btn-primary" type="button" onClick={submit}>Cadastrar</button>
        <button className="btn btn-outline" type="button" onClick={clear}>Limpar</button>
      </div>
    </>
  )
}
