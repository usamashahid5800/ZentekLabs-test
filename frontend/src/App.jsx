import { useEffect, useRef, useState } from 'react'

const defaultApiUrl = import.meta.env.VITE_API_BASE_URL ?? 'https://localhost:7236'
const tokenStorageKey = 'products_api_token'

function IconPlus() {
  return (
    <svg viewBox="0 0 24 24" aria-hidden="true">
      <path d="M12 5v14M5 12h14" />
    </svg>
  )
}

function IconEdit() {
  return (
    <svg viewBox="0 0 24 24" aria-hidden="true">
      <path d="M4 20h4l10-10-4-4L4 16v4zM13 7l4 4" />
    </svg>
  )
}

function IconDelete() {
  return (
    <svg viewBox="0 0 24 24" aria-hidden="true">
      <path d="M4 7h16M10 11v6M14 11v6M6 7l1 13h10l1-13M9 7V4h6v3" />
    </svg>
  )
}

function IconChevronDown() {
  return (
    <svg viewBox="0 0 24 24" aria-hidden="true">
      <path d="M6 9l6 6 6-6" />
    </svg>
  )
}

function IconLogout() {
  return (
    <svg viewBox="0 0 24 24" aria-hidden="true">
      <path d="M10 17l-5-5 5-5M5 12h11M14 4h5v16h-5" />
    </svg>
  )
}

function App() {
  const [apiUrl] = useState(defaultApiUrl)
  const [username, setUsername] = useState('testuser')
  const [password, setPassword] = useState('P@ssw0rd123')
  const [token, setToken] = useState(localStorage.getItem(tokenStorageKey) ?? '')
  const [products, setProducts] = useState([])
  const [colourFilter, setColourFilter] = useState('')
  const [isSubmitting, setIsSubmitting] = useState(false)
  const [isLoadingProducts, setIsLoadingProducts] = useState(false)
  const [isModalOpen, setIsModalOpen] = useState(false)
  const [isProfileOpen, setIsProfileOpen] = useState(false)
  const [toast, setToast] = useState(null)
  const [newProduct, setNewProduct] = useState({ name: '', colour: '', price: '' })
  const profileRef = useRef(null)

  const isAuthenticated = token.trim().length > 0

  useEffect(() => {
    if (token) {
      localStorage.setItem(tokenStorageKey, token)
      void loadProducts('')
    } else {
      localStorage.removeItem(tokenStorageKey)
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [token])

  useEffect(() => {
    if (!toast) {
      return
    }

    const timer = window.setTimeout(() => setToast(null), 3200)
    return () => window.clearTimeout(timer)
  }, [toast])

  useEffect(() => {
    function handleOutsideClick(event) {
      if (profileRef.current && !profileRef.current.contains(event.target)) {
        setIsProfileOpen(false)
      }
    }

    window.addEventListener('mousedown', handleOutsideClick)
    return () => window.removeEventListener('mousedown', handleOutsideClick)
  }, [])

  function showToast(message, type = 'info') {
    setToast({ message, type })
  }

  async function login(event) {
    event.preventDefault()
    setIsSubmitting(true)

    try {
      const response = await fetch(`${apiUrl}/auth/token`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ username, password }),
      })

      if (!response.ok) {
        throw new Error('Login failed. Use valid credentials.')
      }

      const data = await response.json()
      setToken(data.accessToken)
      showToast('Login successful.', 'success')
    } catch (error) {
      showToast(error.message, 'error')
    } finally {
      setIsSubmitting(false)
    }
  }

  async function loadProducts(forcedColour = null) {
    if (!isAuthenticated) {
      return
    }

    const targetColour = forcedColour === null ? colourFilter : forcedColour
    const query = targetColour.trim() ? `?colour=${encodeURIComponent(targetColour.trim())}` : ''

    setIsLoadingProducts(true)
    try {
      const response = await fetch(`${apiUrl}/products${query}`, {
        headers: { Authorization: `Bearer ${token}` },
      })

      if (!response.ok) {
        throw new Error(`Failed to load products (${response.status}).`)
      }

      const data = await response.json()
      setProducts(data)
      showToast(`Products loaded: ${data.length}`, 'success')
    } catch (error) {
      showToast(error.message, 'error')
    } finally {
      setIsLoadingProducts(false)
    }
  }

  async function addProduct(event) {
    event.preventDefault()
    setIsSubmitting(true)

    try {
      const response = await fetch(`${apiUrl}/products`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${token}`,
        },
        body: JSON.stringify({
          name: newProduct.name,
          colour: newProduct.colour,
          price: Number(newProduct.price),
        }),
      })

      if (!response.ok) {
        throw new Error('Unable to add product. Check product fields.')
      }

      setNewProduct({ name: '', colour: '', price: '' })
      setIsModalOpen(false)
      await loadProducts()
      showToast('Product added successfully.', 'success')
    } catch (error) {
      showToast(error.message, 'error')
    } finally {
      setIsSubmitting(false)
    }
  }

  function clearFilter() {
    setColourFilter('')
    void loadProducts('')
  }

  function logout() {
    setIsProfileOpen(false)
    setToken('')
    setProducts([])
    setColourFilter('')
    showToast('Signed out.', 'info')
  }

  if (!isAuthenticated) {
    return (
      <main className="login-layout">
        {toast && <div className={`toast ${toast.type}`}>{toast.message}</div>}

        <section className="login-brand">
          <div className="brand-mark">E</div>
          <h1>E-Commerce Admin Portal</h1>
          <p className="about-copy">
            Secure portal for product operations. Sign in to manage catalog items, monitor inventory color groups, and maintain your store data.
          </p>
          <ul className="about-list">
            <li>Secure JWT authentication</li>
            <li>Admin product management</li>
            <li>SQL Server integrated backend API</li>
          </ul>
        </section>

        <section className="login-card-wrap">
          <section className="login-card">
            <h2>Log In</h2>
            <form onSubmit={login}>
              <label htmlFor="username">Email Address</label>
              <input
                id="username"
                type="text"
                value={username}
                onChange={(event) => setUsername(event.target.value)}
                required
              />

              <label htmlFor="password">Password</label>
              <input
                id="password"
                type="password"
                value={password}
                onChange={(event) => setPassword(event.target.value)}
                required
              />

              <button className="login-submit" type="submit" disabled={isSubmitting}>
                {isSubmitting ? 'Signing in...' : 'Log In'}
              </button>
              <button className="link-btn" type="button">Forgot Password?</button>
              <p className="register-row">
                Don&apos;t have an account? <button className="inline-link" type="button">Register here</button>
              </p>
              <p className="hint">Demo credentials: testuser / P@ssw0rd123</p>
            </form>
          </section>
        </section>
      </main>
    )
  }

  return (
    <main className="admin-shell">
      {toast && <div className={`toast ${toast.type}`}>{toast.message}</div>}

      <nav className="nav-shell">
        <div className="nav-brand">
          <span className="nav-brand-mark">E</span>
          <span>E-Commerce Admin</span>
        </div>

        <div className="nav-right">
          <div className="profile-wrap" ref={profileRef}>
            <button className="profile-btn" type="button" onClick={() => setIsProfileOpen((prev) => !prev)}>
              <span className="avatar">AD</span>
              <span className="profile-text">
                <strong>Administrator</strong>
                <small>{username}</small>
              </span>
              <span className="icon-small"><IconChevronDown /></span>
            </button>

            {isProfileOpen && (
              <div className="profile-menu">
                <button className="danger-item" type="button" onClick={logout}>
                  <span className="icon-small"><IconLogout /></span>
                  Logout
                </button>
              </div>
            )}
          </div>
        </div>
      </nav>

      <section className="dashboard-layout">
        <header className="topbar">
          <div>
            <p className="crumbs">Home {'>'} Products</p>
            <h1>Product Management</h1>
            <p className="sub">Manage product catalog and filter product records by colour.</p>
          </div>
        </header>

        <section className="panel">
          <div className="toolbar">
            <div className="toolbar-group">
              <label htmlFor="colourFilter">Colour Filter</label>
              <input
                id="colourFilter"
                value={colourFilter}
                onChange={(event) => setColourFilter(event.target.value)}
                placeholder="e.g. blue"
              />
              <button type="button" onClick={() => void loadProducts()}>Apply Filter</button>
              <button className="ghost" type="button" onClick={clearFilter}>Clear Filter</button>
            </div>

            <div className="toolbar-group actions-only">
              <button className="add-btn" type="button" onClick={() => setIsModalOpen(true)}>
                <span className="icon-small"><IconPlus /></span>
                Add Product
              </button>
            </div>
          </div>

          <div className="table-wrap">
            <table>
              <thead>
                <tr>
                  <th>Product Name</th>
                  <th>Colour</th>
                  <th>Price</th>
                  <th>Created</th>
                  <th>Status</th>
                  <th>Actions</th>
                </tr>
              </thead>
              <tbody>
                {isLoadingProducts ? (
                  <tr>
                    <td colSpan="6" className="empty">Loading...</td>
                  </tr>
                ) : products.length === 0 ? (
                  <tr>
                    <td colSpan="6" className="empty">No products found.</td>
                  </tr>
                ) : (
                  products.map((product) => (
                    <tr key={product.id}>
                      <td>{product.name}</td>
                      <td>{product.colour}</td>
                      <td>${Number(product.price).toFixed(2)}</td>
                      <td>{new Date(product.createdAtUtc).toLocaleString()}</td>
                      <td>
                        <span className="badge">Active</span>
                      </td>
                      <td>
                        <div className="row-actions">
                          <button className="icon-action" type="button" aria-label="edit product">
                            <IconEdit />
                          </button>
                          <button className="icon-action" type="button" aria-label="delete product">
                            <IconDelete />
                          </button>
                        </div>
                      </td>
                    </tr>
                  ))
                )}
              </tbody>
            </table>
          </div>
        </section>
      </section>

      {isModalOpen && (
        <div className="modal-backdrop" onClick={() => setIsModalOpen(false)} role="presentation">
          <div className="modal-card" onClick={(event) => event.stopPropagation()} role="dialog" aria-modal="true">
            <h3>Add New Product</h3>
            <form onSubmit={addProduct}>
              <label htmlFor="newName">Product Name</label>
              <input
                id="newName"
                value={newProduct.name}
                onChange={(event) => setNewProduct((prev) => ({ ...prev, name: event.target.value }))}
                required
              />

              <label htmlFor="newColour">Colour</label>
              <input
                id="newColour"
                value={newProduct.colour}
                onChange={(event) => setNewProduct((prev) => ({ ...prev, colour: event.target.value }))}
                required
              />

              <label htmlFor="newPrice">Price</label>
              <input
                id="newPrice"
                type="number"
                min="0.01"
                step="0.01"
                value={newProduct.price}
                onChange={(event) => setNewProduct((prev) => ({ ...prev, price: event.target.value }))}
                required
              />

              <div className="modal-actions">
                <button className="ghost" type="button" onClick={() => setIsModalOpen(false)}>Cancel</button>
                <button type="submit" disabled={isSubmitting}>
                  {isSubmitting ? 'Saving...' : 'Save Product'}
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </main>
  )
}

export default App
