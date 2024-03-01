import { GetDetailContentAsync } from '@/api/reqContent.js'
import { ref } from 'vue'

const article = ref({
  title: 'title',
  create_by: '-',
  create_at: '-',
  article: '',
  additional_link: ''
})

async function LoadArticleById(articleId) {
  const result = await GetDetailContentAsync(articleId)
  if (result.is_success) {
    for (const key in article.value) {
      if (key === 'article') {
        article.value[key] = JSON.parse(result.content[key])
        continue
      }
      article.value[key] = result.content[key]
    }
    return true
  }
  return false
}

export { article, LoadArticleById }