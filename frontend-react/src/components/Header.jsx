export function Header({ theme, onToggleTheme }) {
  return (
    <header className="header">
      <div className="brand">
        <div className="badge">P+</div>
        <div>
          <div className="title">Produtos</div>
          <div className="subtitle">CRUD com validação e tema claro/escuro</div>
        </div>
      </div>
      <button className="theme-toggle" type="button" onClick={onToggleTheme}>
        Tema: {theme === 'dark' ? 'Escuro' : 'Claro'}
      </button>
    </header>
  )
}
