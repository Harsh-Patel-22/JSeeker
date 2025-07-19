const RoleSelector = ({ value, onChange, disabled }) => (
  <div className="mb-3">
    <label className="form-label">Role</label>
    <select
      name="role"
      className="form-select"
      value={value}
      onChange={onChange}
      required
      disabled={disabled}
    >
      <option value="">Choose a role</option>
      <option value="Seeker">Seeker</option>
      <option value="Hirer">Hirer</option>
    </select>
  </div>
);

export default RoleSelector;

